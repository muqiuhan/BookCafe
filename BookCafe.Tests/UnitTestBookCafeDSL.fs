module BookCafe.Tests.BookCafeTestsDSL

open BookCafe
open FsUnit


let Given (state: State.t) = state
let When (command: Command.t) (state: State.t) = (command, state)

let ThenStateShouldBe (expectedState: State.t) ((command, state): Command.t * State.t) : Event.t =
    let (actualState, event) = CommandHandlers.evolve state command
    actualState |> should equal expectedState
    event


let WithEvents (expectedEvents: Event.t) (actualEvents: Event.t) : unit =
    actualEvents |> should equal expectedEvents
