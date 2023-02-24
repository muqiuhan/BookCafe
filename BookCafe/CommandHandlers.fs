module BookCafe.CommandHandlers

open Chessie.ErrorHandling
open Domain

module Handler =
    let (|NonOrderedBook|_|) order book =
        match List.contains book order.Books with
        | false -> Some book
        | true -> None

    let (|NonOrderedDrink|_|) order drink =
        match List.contains drink order.Drinks with
        | false -> Some drink
        | true -> None

    let (|ServeBookCompletesOrder|_|) order book =
        match isServingBookCompletesOrder order book with
        | true -> Some book
        | false -> None

    let PrepareDrink drink tabID =
        function
        | State.PlacedOrder order ->
            match drink with
            | NonOrderedDrink order _ -> Error.CanNotPrepareNonOrderedDrink drink |> fail
            | _ -> [ Event.DrinkPrepared(drink, tabID) ] |> ok
        | State.ServedOrder _ -> Error.OrderAlreadyServed |> fail
        | State.OpenedTab _ -> Error.CanNotPrepareForNonPlacedOrder |> fail
        | State.ClosedTab _ -> Error.CanNotPrepareWithClosedTab |> fail
        | _ -> failwith "TODO"

    let ServeBook book tabID =
        function
        | State.PlacedOrder order ->
            let event = Event.BookServed(book, tabID)

            match book with
            | NonOrderedBook order _ -> Error.CanNotServeNonOrderedBook book |> fail
            | ServeBookCompletesOrder order _ ->
                event
                :: [ Event.OrderServed(
                         order,
                         { Tab = order.Tab
                           Amount = orderAmount order }
                     ) ]
                |> ok
            | _ -> [ event ] |> ok
        | State.ServedOrder _ -> Error.OrderAlreadyServed |> fail
        | State.OpenedTab _ -> Error.CanNotServeForNonPlacedOrder |> fail
        | State.ClosedTab _ -> Error.CanNotServeWithClosedTab |> fail
        | _ -> failwith "TODO"

    let PlaceOrder order =
        function
        | State.OpenedTab _ ->
            if List.isEmpty order.Books && List.isEmpty order.Drinks then
                fail Error.CanNotPlaceEmptyOrder
            else
                [ Event.OrderPlaced order ] |> ok
        | State.ClosedTab _ -> fail Error.CanNotOrderWithClosedTab
        | _ -> fail Error.OrderAlreadyPlaced

    let OpenTab tab =
        function
        | State.ClosedTab _ -> [ Event.TabOpened tab ] |> ok
        | _ -> Error.TabAlreadyOpened |> fail

let apply = State.apply

let execute state command =
    match command with
    | Command.OpenTab tab -> Handler.OpenTab tab state
    | Command.PlaceOrder order -> Handler.PlaceOrder order state
    | Command.ServeBook (book, tabID) -> Handler.ServeBook book tabID state
    | Command.PrepareDrink (drink, tabID) -> Handler.PrepareDrink drink tabID state
    | _ -> failwith "TODO"

let evolve state command =
    match execute state command with
    | Ok (event, _) -> (List.fold apply state event, event) |> ok
    | Bad err -> Bad err
