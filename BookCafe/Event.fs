module BookCafe.Event

open System

type t =
    | TabOpened of Domain.Tab
    | OrderPlaced of Domain.Order
    | DrinkServed of Domain.Drink * Guid
    | BookPrepared of Domain.Book * Guid
    | BookServed of Domain.Book * Guid
    | OrderServed of Domain.Order * Domain.Payment
    | TabClosed of Domain.Payment
