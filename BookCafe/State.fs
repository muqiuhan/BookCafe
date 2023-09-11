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

module BookCafe.State

open BookCafe.Event
open BookCafe.Domain
open System

type State =
    | ClosedTab of option<Guid>
    | OpenedTab of Tab
    | PlacedOrder of Order
    | OrderInProgress of InProgressOrder
    | ServedOrder of Order

let Apply (state : State) (event : Event) : State =
    match (state, event) with
    | ClosedTab _, TabOpened tab -> OpenedTab tab
    | OpenedTab _, OrderPlaced order -> PlacedOrder order
    | PlacedOrder order, BookServed(item, _tabID) ->
        { PlacedOrder = order
          ServedBooks = [ item ]
          ServedDrinks = []
          PrepareDrinks = [] }
        |> OrderInProgress
    | OrderInProgress _inProgressOrder, OrderServed(order, _payment) -> ServedOrder order
    | _ -> state
