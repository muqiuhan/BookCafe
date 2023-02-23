module BookCafe.Command

open System

type t =
    | OpenTab of Domain.Tab
    | PlaceOrder of Domain.Order
    | ServeBook of Domain.Book * Guid
    | PrepareDrink of Domain.Drink * Guid
    | ServeDrink of Domain.Drink * Guid
    | CloseTab of Domain.Payment
