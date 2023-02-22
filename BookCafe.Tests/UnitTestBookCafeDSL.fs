module BookCafe.Tests.BookCafeTestsDSL

open BookCafe
open NUnit.Framework
open FsUnit
open Chessie.ErrorHandling

let Given (state: State.t) = state
let When (command: Command.t) (state: State.t) = (command, state)

let ThenStateShouldBe (expectedState: State.t) ((command, state): Command.t * State.t) : Event.t list option =
    match CommandHandlers.evolve state command with
    | Ok ((actualState, event), _) ->
        actualState |> should equal expectedState
        Some event
    | Bad errs ->
        Assert.Fail $"Expected : {expectedState}, But Actual: {errs.Head}"
        None

let WithEvents (expectedEvents: Event.t list) (actualEvents: Event.t list option) : unit =
    match actualEvents with
    | Some actualEvents -> actualEvents |> should equal expectedEvents
    | None -> None |> should equal expectedEvents

let ShouldFailWith (expectedError: Error.t) ((command, state): Command.t * State.t) =
    match CommandHandlers.evolve state command with
    | Bad errs -> errs.Head |> should equal expectedError
    | Ok (r, _) -> Assert.Fail $"Expected: {expectedError}, But Actual: {r}"
