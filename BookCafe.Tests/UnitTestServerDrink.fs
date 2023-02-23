module BookCafe.Tests.UnitTestServerDrink

open BookCafe
open BookCafe.Domain
open NUnit.Framework
open BookCafeTestsDSL
open UnitTestData

[<Test>]
let ``Can Serve Book`` () =
    let order =
        { order with Books = [ ModernCompilerImplementationInML; RealWorldOCaml ] }

    let expected =
        { PlacedOrder = order
          ServedBooks = [ ModernCompilerImplementationInML ]
          PreparedDrinks = []
          ServedDrinks = [] }

    Given(State.PlacedOrder order)
    |> When(Command.ServeBook(ModernCompilerImplementationInML, order.Tab.ID))
    |> ThenStateShouldBe(State.OrderInProgress expected)
    |> WithEvents [ Event.BookServed(ModernCompilerImplementationInML, order.Tab.ID) ]

[<Test>]
let ``Can not serve non ordered book`` () =
    let order = { order with Books = [] }

    Given(State.PlacedOrder order)
    |> When(Command.ServeBook(ModernCompilerImplementationInML, order.Tab.ID))
    |> ShouldFailWith(Error.CanNotServeNonOrderedBook ModernCompilerImplementationInML)

[<Test>]
let ``Can not serve book for already served order`` () =
    Given(State.ServedOrder order)
    |> When(Command.ServeBook(ModernCompilerImplementationInML, order.Tab.ID))
    |> ShouldFailWith Error.OrderAlreadyServed

[<Test>]
let ``Can not serve book for non placed order`` () =
    Given(State.OpenedTab tab)
    |> When(Command.ServeBook(ModernCompilerImplementationInML, tab.ID))
    |> ShouldFailWith Error.CanNotServeForNonPlacedOrder

[<Test>]
let ``Can not serve with closed tab`` () =
    Given(State.ClosedTab None)
    |> When(Command.ServeBook(ModernCompilerImplementationInML, tab.ID))
    |> ShouldFailWith Error.CanNotServeWithClosedTab

[<Test>]
let ``Can serve book for order containing only one book`` () =
    let order = { order with Books = [ ModernCompilerImplementationInML ] }

    let payment =
        { Tab = order.Tab
          Amount = bookPrice ModernCompilerImplementationInML }

    Given(State.PlacedOrder order)
    |> When(Command.ServeBook(ModernCompilerImplementationInML, order.Tab.ID))
    |> ThenStateShouldBe(State.ServedOrder order)
    |> WithEvents
        [ Event.BookServed(ModernCompilerImplementationInML, order.Tab.ID)
          Event.OrderServed(order, payment) ]
