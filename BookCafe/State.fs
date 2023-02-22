module BookCafe.State

open System

type t =
    | ClosedTab of Guid option
    | OpenedTab of Domain.Tab
    | PlacedOrder of Domain.Order
    | OrderInProgress of Domain.InProgressOrder
    | ServedOrder of Domain.Order
