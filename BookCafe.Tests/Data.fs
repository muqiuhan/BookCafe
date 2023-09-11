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

module BookCafe.Tests.Data

open BookCafe.Domain
open System

let tab = { ID = Guid.NewGuid(); TableNumber = 1 }

let RealWorldOCaml =
    Book
        { MenuNumber = 1
          Name = "Real World OCaml"
          Price = 1.5m }

let ModernCompilerDesign =
    Book
        { MenuNumber = 3
          Name = "ModernCompilerDesign"
          Price = 1.0m }

let PLConcetps =
    Book
        { MenuNumber = 5
          Name = "PL Concepts"
          Price = 1.3m }

let coke =
    Drink
        { MenuNumber = 1
          Name = "Coke"
          Price = 1.5m }

let lemonade =
    Drink
        { MenuNumber = 3
          Name = "Lemonade"
          Price = 1.0m }

let appleJuice =
    Drink
        { MenuNumber = 5
          Name = "Apple Juice"
          Price = 1.3m }

let order = { Tab = tab; Books = []; Drinks = [] }

let BookPrice (Book(book)) : decimal = book.Price
let DrinkPrice (Drink(drink)) : decimal = drink.Price
