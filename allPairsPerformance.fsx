let inline collectTheGarbage() =
    System.GC.Collect()
    System.GC.WaitForPendingFinalizers()
    System.GC.Collect()

let time description f =
    collectTheGarbage()
    let sw = System.Diagnostics.Stopwatch.StartNew()
    f()
    collectTheGarbage()
    sw.Stop()
    printfn "%s took %O" description sw.Elapsed

let repeat iterations action =
    for i = 1 to iterations do
        action () |> ignore

let timeAll iterations xs ys =
    List.allPairs xs ys |> ignore
    time "New List -> List " (fun() -> repeat iterations (fun _ -> List.allPairs xs ys))

    List.allPairs2 xs ys |> ignore
    time "Old List -> List " (fun() -> repeat iterations (fun _ -> List.allPairs2 xs ys))

    let xa, ya = (xs |> Array.ofSeq), (ys |> Array.ofSeq)
    Array.allPairs xa ya |> ignore
    time "   Array -> Array" (fun() -> repeat iterations (fun _ -> Array.allPairs xa ya))

    Seq.allPairs xs ys |> Seq.iter ignore
    time "     List -> Seq  " (fun() -> repeat iterations (fun _ -> Seq.allPairs xs ys |> List.ofSeq))

    Seq.allPairs xa ya |> Seq.iter ignore
    time "    Array -> Seq  " (fun() -> repeat iterations (fun _ -> Seq.allPairs xa ya |> List.ofSeq))

    printfn ""

//#time "on"

timeAll 1000000 [1] [1]
timeAll 1000 [1..100] [1..100]
timeAll 1 [1..1000] [1..1000]

//#time "off"
