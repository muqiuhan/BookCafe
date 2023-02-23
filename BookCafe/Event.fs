module BookCafe.Event

open System

type t =
    | TabOpened of Domain.Tab
    | OrderPlaced of Domain.Order
    | BookServed of Domain.Book * Guid
    | DrinkPrepared of Domain.Drink * Guid
    | DrinkServed of Domain.Drink * Guid
    | OrderServed of Domain.Order * Domain.Payment
    | TabClosed of Domain.Payment
