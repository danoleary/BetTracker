module Result

let bind switchFn twoTrackInput =
    match twoTrackInput with
    | Ok success -> switchFn success
    | Error failure -> Error failure

let map f aResult =
    match aResult with
    | Ok success -> Ok (f success)
    | Error failure -> Error failure