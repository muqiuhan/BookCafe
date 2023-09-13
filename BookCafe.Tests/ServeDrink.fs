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

module BookCafe.Tests.ServeDrink

open BookCafe.Domain
open BookCafe.State
open BookCafe.Command
open BookCafe.Event
open BookCafe.Error
open BookCafe.Tests.DSL
open BookCafe.Tests.Data

open NUnit.Framework

[<Test>]
let ``Remain in order in progress while serving book`` () =
    let order =
        { order with
            Books = [ RealWorldOCaml; ModernCompilerDesign; PLConcetps ] }

    let orderInProgress =
        { PlacedOrder = order
          ServedBooks = [ RealWorldOCaml ]
          PrepareDrinks = []
          ServedDrinks = [] }

    let expected =
        { orderInProgress with
            ServedBooks = ModernCompilerDesign :: orderInProgress.ServedBooks }

    Given(OrderInProgress orderInProgress)
    |> When(ServeBook(ModernCompilerDesign, order.Tab.ID))
    |> ThenStateShouldBe(OrderInProgress expected)
    |> WithEvents [ BookServed(ModernCompilerDesign, order.Tab.ID) ]

[<Test>]
let ``Can not serve non ordered books during order in progress `` () =
    let order =
        { order with
            Books = [ RealWorldOCaml; ModernCompilerDesign ] }

    let orderInProgress =
        { PlacedOrder = order
          ServedBooks = [ RealWorldOCaml ]
          PrepareDrinks = []
          ServedDrinks = [] }

    Given(OrderInProgress orderInProgress)
    |> When(ServeBook(PLConcetps, order.Tab.ID))
    |> ShouldFailWith(CanNotServeNonOrderedBook PLConcetps)

[<Test>]
let ``Can not serve an already served books`` () =
    let order =
        { order with
            Books = [ RealWorldOCaml; ModernCompilerDesign ] }

    let orderInProgress =
        { PlacedOrder = order
          ServedBooks = [ RealWorldOCaml ]
          PrepareDrinks = []
          ServedDrinks = [] }

    Given(OrderInProgress orderInProgress)
    |> When(ServeBook(RealWorldOCaml, order.Tab.ID))
    |> ShouldFailWith(CanNotServeAlreadyServedBook RealWorldOCaml)
