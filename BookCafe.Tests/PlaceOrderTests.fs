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

module BookCafe.Tests.PlaceOrder

open System
open NUnit.Framework
open BookCafe.Tests.DSL
open BookCafe.Domain
open BookCafe.Event
open BookCafe.Command
open BookCafe.State
open BookCafe.Error

let tab = { ID = Guid.NewGuid(); TableNumber = 1 }

let RealWorldOCaml =
    Book
        { MenuNumber = 1
          Name = "Real World OCaml"
          Price = 1.5m }

let order = { Tab = tab; Drinks = []; Books = [] }

[<Test>]
let ``Can place only books order`` () =
    let order =
        { order with
            Books = [ RealWorldOCaml ] } in

    Given(OpenedTab tab)
    |> When(PlaceOrder order)
    |> ThenStateShouldBe(PlaceedOrder order)
    |> WithEvents [ OrderPlaced order ]

[<Test>]
let ``Can not place empty order`` () =
    Given(OpenedTab tab)
    |> When(PlaceOrder order)
    |> ShouldFailWith CanNotPlaceEmptyOrder

[<Test>]
let ``Can not place order with a closed tab`` () =
    let order =
        { order with
            Books = [ RealWorldOCaml ] }

    Given(ClosedTab None)
    |> When(PlaceOrder order)
    |> ShouldFailWith CanNotOrderWithClosedTab

[<Test>]
let ``Can not place order multiple times`` () =
    let order =
        { order with
            Books = [ RealWorldOCaml ] }

    Given(PlaceedOrder order)
    |> When(PlaceOrder order)
    |> ShouldFailWith OrderAlreadyPlaced
