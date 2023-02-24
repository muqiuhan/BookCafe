module BookCafe.State

open System
open Domain

type t =
    | ClosedTab of Guid option
    | OpenedTab of Tab
    | PlacedOrder of Order
    | OrderInProgress of InProgressOrder
    | ServedOrder of Order

let apply state event =
    match state, event with
    | ClosedTab _, Event.TabOpened tab -> OpenedTab tab
    | OpenedTab _, Event.OrderPlaced order -> PlacedOrder order
    | PlacedOrder order, Event.BookServed (book, _) ->
        { PlacedOrder = order
          ServedDrinks = []
          ServedBooks = [ book ]
          PreparedDrinks = [] }
        |> OrderInProgress
    | PlacedOrder order, Event.DrinkPrepared (drink, _) ->
        { PlacedOrder = order
          ServedDrinks = []
          ServedBooks = []
          PreparedDrinks = [ drink ] }
        |> OrderInProgress
    | OrderInProgress _, Event.OrderServed (order, _) -> ServedOrder order
    | _ -> state
