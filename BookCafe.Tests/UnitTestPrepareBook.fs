module BookCafe.Tests.UnitTestPrepareBook

open BookCafe
open BookCafe.Domain
open NUnit.Framework
open BookCafeTestsDSL
open UnitTestData

[<Test>]
let ``Can Prepare Drink`` () =
    let order = { order with Drinks = [ tea ] }

    let expected =
        { PlacedOrder = order
          ServedBooks = []
          PreparedDrinks = [ tea ]
          ServedDrinks = [] }

    Given(State.PlacedOrder order)
    |> When(Command.PrepareDrink(tea, order.Tab.ID))
    |> ThenStateShouldBe(State.OrderInProgress expected)
    |> WithEvents [ Event.DrinkPrepared(tea, order.Tab.ID) ]

[<Test>]
let ``Can not prepare a non-ordered drink`` () =
    let order = { order with Drinks = [ tea ] }

    Given(State.PlacedOrder order)
    |> When(Command.PrepareDrink(lemonade, order.Tab.ID))
    |> ShouldFailWith(Error.CanNotPrepareNonOrderedDrink lemonade)

[<Test>]
let ``Can not prepare a drink for served order`` () =
    Given(State.ServedOrder order)
    |> When(Command.PrepareDrink(tea, order.Tab.ID))
    |> ShouldFailWith Error.OrderAlreadyServed

[<Test>]
let ``Can not prepare with closed tab`` () =
    Given(State.ClosedTab None)
    |> When(Command.PrepareDrink(tea, order.Tab.ID))
    |> ShouldFailWith Error.CanNotPrepareWithClosedTab
