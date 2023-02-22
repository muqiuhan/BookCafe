module BookCafe.State

open System

type t =
    | ClosedTab of Guid option
    | OpenedTab of Domain.Tab
    | PlacedOrder of Domain.Order
    | OrderInProgress of Domain.InProgressOrder
    | ServedOrder of Domain.Order

let apply state event =
    match state, event with
    | ClosedTab _, Event.TabOpened tab -> OpenedTab tab
    | OpenedTab _, Event.OrderPlaced order -> PlacedOrder order
    | _ -> state
