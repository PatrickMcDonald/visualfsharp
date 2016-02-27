let inline collectTheGarbage() =
    System.GC.Collect()
    System.GC.WaitForPendingFinalizers()
    System.GC.Collect()

let repeat iterations action =
    for i = 1 to iterations do
        action () |> ignore

let time description iterations f =
    // warm up
    f() |> ignore

    // start with a clean garbage heap
    collectTheGarbage()

    // time the repeated iterations plus the final garbage collection
    let sw = System.Diagnostics.Stopwatch.StartNew()
    repeat iterations f
    collectTheGarbage()
    sw.Stop()

    // print the results
    printfn "%s took %O" description sw.Elapsed

let timeAll iterations xs ys =
    let xa, ya = (xs |> Array.ofSeq), (ys |> Array.ofSeq)
    time "  List |> List   " iterations (fun _ -> List.allPairs xs ys)
    time "  List |> List 2 " iterations (fun _ -> List.allPairs2 xs ys)
    time " Array |> Array  " iterations (fun _ -> Array.allPairs xa ya)
    time " Array |> Array 2" iterations (fun _ -> Array.allPairs2 xa ya)
//    time "  List -> Seq    " iterations (fun _ -> Seq.allPairs xs ys |> List.ofSeq)
//    time " Array -> Seq    " iterations (fun _ -> Seq.allPairs xa ya |> List.ofSeq)

    printfn ""

//#time "on"

timeAll 1000000 [1] [1]
timeAll    1000 [1..100] [1..100]
timeAll       1 [1..1000] [1..1000]

//#time "off"
