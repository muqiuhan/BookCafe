module BookCafe.Error

open Domain

type t =
    | TabAlreadyOpened
    | CanNotPlaceEmptyOrder
    | CanNotOrderWithClosedTab
    | OrderAlreadyPlaced
    | CanNotServeNonOrderedBook of Book
    | OrderAlreadyServed
    | CanNotServeForNonPlacedOrder
    | CanNotServeWithClosedTab
