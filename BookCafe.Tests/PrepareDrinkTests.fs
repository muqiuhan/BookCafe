(* The MIT License (MIT)
 * 
 * Copyright (c) 2022 Muqiu Han
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *)

module BookCafe.Tests.PrepareDrink

open BookCafe.Domain
open BookCafe.State
open BookCafe.Command
open BookCafe.Event
open BookCafe.Error
open BookCafe.Tests.DSL
open BookCafe.Tests.Data

open NUnit.Framework

[<Test>]
let ``Can prepare drink`` () =
    let order = { order with Drinks = [ coke ] } in

    let expected =
        { PlacedOrder = order
          ServedBooks = []
          PrepareDrinks = [ coke ]
          ServedDrinks = [] }

    Given(PlacedOrder order)
    |> When(PrepareDrink(coke, order.Tab.ID))
    |> ThenStateShouldBe(OrderInProgress expected)
    |> WithEvents [ DrinkPrepared(coke, order.Tab.ID) ]

[<Test>]
let ``Can not prepare a non-ordered drink`` () =
    let order = { order with Drinks = [ coke ] }

    Given(PlacedOrder order)
    |> When(PrepareDrink(lemonade, order.Tab.ID))
    |> ShouldFailWith(CanNotPrepareNonOrderedDrink lemonade)

[<Test>]
let ``Can not prepare a drink for served order`` () =
    Given(ServedOrder order)
    |> When(PrepareDrink(coke, order.Tab.ID))
    |> ShouldFailWith OrderAlreadyServed

[<Test>]
let ``Can not prepare with closed tab`` () =
    Given(ClosedTab None)
    |> When(PrepareDrink(coke, order.Tab.ID))
    |> ShouldFailWith CanNotPrepareWithClosedTab
