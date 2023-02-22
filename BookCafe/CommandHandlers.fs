module BookCafe.CommandHandlers


let apply state event =
    match state, event with
    | State.ClosedTab _, Event.TabOpened tab -> State.OpenedTab tab
    | _ -> state

let execute state command =
    match command with
    | Command.OpenTab tab -> Event.TabOpened tab
    | _ -> failwith "TODO"

let evolve state command =
    let event = execute state command
    let newState = apply state event
    (newState, event)
