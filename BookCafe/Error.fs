module BookCafe.Error

type t =
    | TabAlreadyOpened
    | CanNotPlaceEmptyOrder
    | CanNotOrderWithClosedTab
    | OrderAlreadyPlaced
