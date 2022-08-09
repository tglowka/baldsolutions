module tail_call 

open System
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

module StandardRecursion=
    let rec factorial x =
        match x with
        | var1 when var1 < 1 -> 1
        | x -> x * factorial (x-1)

module TailRecursion  =    
    let factorial x = 
       let rec factorial2 x acc =
            match x with
            | 0 -> acc
            | x -> factorial2 (x-1) (acc * x)
       factorial2 x 1

//[<MemoryDiagnoser>]
//type Benchmarks () =
//    [<Params(10000, 20000, 30000)>]
//    member val public count = 0 with get, set

//    [<Benchmark>]
//    member this.StandardRecursion () = StandardRecursion.factorial this.count

//    [<Benchmark>]
//    member this.TailRecursion () = TailRecursion.factorial this.count


[<EntryPoint>]
let main argv =
    //BenchmarkRunner.Run<Benchmarks>() |> ignore
    
    //Only StandardRecursion throws StackOverflowException
    Console.WriteLine( StandardRecursion.factorial 6)
    Console.WriteLine( TailRecursion.factorial 6)
    0