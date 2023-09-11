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

module BookCafe.Tests.CommandHandlers

open BookCafe.State
open BookCafe.Command
open BookCafe.Event
open BookCafe.Domain
open BookCafe.Error

open System
open Chessie.ErrorHandling

module Handlers =
    let OpenTab (tab : Tab) (state : State) : Result<list<Event>, Error> =
        match state with
        | ClosedTab _ -> [ TabOpened tab ] |> ok
        | _ -> TabAlreadyOpened |> fail

    let PlaceOrder (order : Order) (state : State) : Result<list<Event>, Error> =
        match state with
        | OpenedTab _ ->
            if List.isEmpty order.Books && List.isEmpty order.Drinks then
                CanNotPlaceEmptyOrder |> fail
            else
                [ OrderPlaced order ] |> ok
        | ClosedTab _ -> CanNotOrderWithClosedTab |> fail
        | _ -> OrderAlreadyPlaced |> fail

let Execute (state : State) (command : Command) : Result<list<Event>, Error> =
    match command with
    | OpenTab tab -> Handlers.OpenTab tab state
    | PlaceOrder order -> Handlers.PlaceOrder order state
    | _ -> failwith "TODO"

/// State transformation
let Evolve (state : State) (command : Command) : Result<State * list<Event>, Error> =
    match (Execute state command) with
    | Ok(events, _msg) -> (List.fold Apply state events, events) |> ok
    | Bad err -> Bad err
