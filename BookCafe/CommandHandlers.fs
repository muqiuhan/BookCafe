module BookCafe.CommandHandlers

open Chessie.ErrorHandling

module Handler =
    let PlaceOrder (order: Domain.Order) =
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
    | _ -> failwith "TODO"

let evolve state command =
    match execute state command with
    | Ok (event, _) -> (List.fold apply state event, event) |> ok
    | Bad err -> Bad err
