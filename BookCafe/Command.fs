module BookCafe.Command

open System

type t =
    | OpenTab of Domain.Tab
    | PlaceOrder of Domain.Order
    | ServeDrink of Domain.Drink * Guid
    | PrepareBook of Domain.Book * Guid
    | ServeFood of Domain.Book * Guid
    | CloseTab of Domain.Payment
