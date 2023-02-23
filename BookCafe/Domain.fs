module BookCafe.Domain

open System

type Tab = { ID: Guid; TableNumber: int }

type Item =
    { MenuNumber: int
      Price: decimal
      Name: string }

type Book = Book of Item
type Drink = Drink of Item

type Payment = { Tab: Tab; Amount: decimal }

type Order =
    { Books: Book list
      Drinks: Drink list
      Tab: Tab }

type InProgressOrder =
    { PlacedOrder: Order
      ServedDrinks: Drink list
      ServedBooks: Book list
      PreparedDrinks: Drink list }

let isServingBookCompletesOrder order book =
    List.isEmpty order.Drinks && order.Books = [ book ]

let orderAmount order =
    (order.Drinks |> List.map (fun (Drink drink) -> drink.Price) |> List.sum)
    + (order.Books |> List.map (fun (Book book) -> book.Price) |> List.sum)
