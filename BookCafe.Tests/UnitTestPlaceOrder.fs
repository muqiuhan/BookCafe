module BookCafe.Tests.UnitTestPlaceOrder

open System
open BookCafe
open BookCafe.Domain
open NUnit.Framework
open BookCafeTestsDSL

let tab = { ID = Guid.NewGuid(); TableNumber = 1 }

let tea =
    Drink
        { MenuNumber = 1
          Name = "Tea"
          Price = 12.5m }

let order = { Tab = tab; Drinks = []; Books = [] }

[<Test>]
let ``Can place only drinks order`` () =
    let order = { order with Drinks = [ tea ] }

    Given(State.OpenedTab tab)
    |> When(Command.PlaceOrder order)
    |> ThenStateShouldBe(State.PlacedOrder order)
    |> WithEvents [ Event.OrderPlaced order ]

[<Test>]
let ``Can not place empty order`` () =
    Given(State.OpenedTab tab)
    |> When(Command.PlaceOrder order)
    |> ShouldFailWith Error.CanNotPlaceEmptyOrder

[<Test>]
let ``Can not place order with a closed tab`` () =
    let order = { order with Drinks = [ tea ] }

    Given(State.ClosedTab None)
    |> When(Command.PlaceOrder order)
    |> ShouldFailWith Error.CanNotOrderWithClosedTab

[<Test>]
let ``Can not place order multiple times`` () =
    let order = { order with Drinks = [ tea ] }

    Given(State.PlacedOrder order)
    |> When(Command.PlaceOrder order)
    |> ShouldFailWith Error.OrderAlreadyPlaced
