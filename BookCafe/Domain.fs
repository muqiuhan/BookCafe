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

module BookCafe.Domain

open System

type Tab = { ID : Guid; TableNumber : int }

type Item =
    { MenuNumber : int
      Price : decimal
      Name : string }

type Drink = Drink of Item
type Book = Book of Item

type Payment = { Tab : Tab; Amount : decimal }

type Order =
    { Drinks : list<Drink>
      Books : list<Book>
      Tab : Tab }

type InProgressOrder =
    { PlacedOrder : Order
      ServedBooks : list<Book>
      ServedDrinks : list<Drink>
      PrepareDrinks : list<Drink> }

let IsServingBookCompletesOrder (order : Order) (book : Book) : bool =
    List.isEmpty order.Drinks && order.Books = [ book ]

let OrderAmount (order : Order) : decimal =
    let drinkAmount =
        order.Drinks |> List.map (fun (Drink drink) -> drink.Price) |> List.sum

    let bookAmount =
        order.Books |> List.map (fun (Book book) -> book.Price) |> List.sum

    bookAmount + drinkAmount
