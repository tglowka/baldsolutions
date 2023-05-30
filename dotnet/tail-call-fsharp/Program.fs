module tail_call 

open System

module StandardRecursion=
    let rec factorial x =
        if x < 1 then 1
        else x * factorial (x-1)

module TailRecursion  =    
    let factorial x = 
       let rec factorial2 x acc =
            if x = 0 then acc
            else factorial2 (x-1) (acc * x)
       factorial2 x 1

[<EntryPoint>]
let main argv =
    StandardRecursion.factorial 6000000
    TailRecursion.factorial 6000000
    0