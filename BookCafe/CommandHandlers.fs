module BookCafe.CommandHandlers

open Chessie.ErrorHandling

let apply state event =
    match state, event with
    | State.ClosedTab _, Event.TabOpened tab -> State.OpenedTab tab
    | _ -> state

let execute state command =
    let handleOpenTab tab =
        function
        | State.ClosedTab _ -> [ Event.TabOpened tab ] |> ok
        | _ -> Error.TabAlreadyOpened |> fail in

    match command with
    | Command.OpenTab tab -> handleOpenTab tab state
    | _ -> failwith "TODO"

let evolve state command =
    match execute state command with
    | Ok (event, _) -> (List.fold apply state event, event) |> ok
    | Bad err -> Bad err
