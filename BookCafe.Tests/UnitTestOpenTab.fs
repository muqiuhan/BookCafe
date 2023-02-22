module BookCafe.Tests.UnitTestOpenTab

open NUnit.Framework
open System
open BookCafe
open BookCafe.Domain
open BookCafeTestsDSL

[<Test>]
let ``Can Open a new Tab`` () =
    let tab = { ID = Guid.NewGuid(); TableNumber = 1 }

    Given(State.ClosedTab None) // Current State
    |> When(Command.OpenTab tab) // Command
    |> ThenStateShouldBe(State.OpenedTab tab) // New State
    |> WithEvents(Event.TabOpened tab) // Event Emitted
