module BookCafe.Tests.UnitTestData

open System
open BookCafe.Domain

let tab = { ID = Guid.NewGuid(); TableNumber = 1 }
let order = { Tab = tab; Books = []; Drinks = [] }

let tea =
    Drink
        { MenuNumber = 1
          Name = "Tea"
          Price = 12.5m }

let lemonade =
    Drink
        { MenuNumber = 1
          Name = "Lemonade"
          Price = 10.5m }

let RealWorldOCaml =
    Book
        { MenuNumber = 1
          Name = "Real World OCaml"
          Price = 1m }

let ModernCompilerImplementationInML =
    Book
        { MenuNumber = 1
          Name = "Modern Compiler Implementation In ML"
          Price = 1m }

let drinkPrice =
    function
    | Drink drink -> drink.Price

let bookPrice =
    function
    | Book book -> book.Price
