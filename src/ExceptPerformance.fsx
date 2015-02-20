open System

let generateSequences upper n m =
    let rand = new Random(12345)
    let array1 = Array.init n (fun _ -> rand.Next upper)
    let array2 = Array.init m (fun _ -> rand.Next upper)
    (Seq.delay <| fun () -> array1 :> seq<_>),  (Seq.delay <| fun () -> array2 :> seq<_>)

let time description iterations f =
    let inline collectTheGarbage() =
        System.GC.Collect()
        System.GC.WaitForPendingFinalizers()
        System.GC.Collect()
    f()
    collectTheGarbage()
    let sw = System.Diagnostics.Stopwatch.StartNew()
    for i = 1 to iterations do
        f()
    collectTheGarbage()
    sw.Stop()
    printfn "%s took %O" description sw.Elapsed
    sw.ElapsedMilliseconds

let ignoreTime description iterations f =
    time description iterations f
    |> ignore

let run iteri s =
    iteri (fun i x -> ignore (i,x)) s

let mapPair f (a, b) = (f a, f b)

let suite iterations upper n m =
    printfn "Iterations = %d, n = %d, m = %d, upper = %d" iterations n m upper

    let s1, s2 = generateSequences upper n m

    let a1,a2 = (s1, s2) |> mapPair Array.ofSeq
    let l1,l2 = (s1, s2) |> mapPair List.ofSeq

    let t0 = time "Enumerable.Except" iterations (fun () ->
        System.Linq.Enumerable.Except(s1, s2)
        |> Seq.iter ignore)

    ignoreTime "Seq.except" iterations (fun () ->
        Seq.except s1 s2
        |> Seq.iter ignore)

    ignoreTime "Array.Except" iterations (fun () ->
        Array.except a1 a2
        |> Seq.iter ignore)

    ignoreTime "Seq.Except (on arrays)" iterations (fun () ->
        Seq.except a1 a2
        |> Seq.iter ignore)

    ignoreTime "Enumerable.Except (on arrays)" iterations (fun () ->
        System.Linq.Enumerable.Except(a1, a2)
        |> Seq.iter ignore)

    ignoreTime "List.Except" iterations (fun () ->
        List.except l1 l2
        |> Seq.iter ignore)

    ignoreTime "Seq.Except (on lists)" iterations (fun () ->
        Seq.except l1 l2
        |> Seq.iter ignore)

    ignoreTime "Enumerable.Except (on lists)" iterations (fun () ->
        System.Linq.Enumerable.Except(l1, l2)
        |> Seq.iter ignore)


suite 1000 (*iterations*) 50 (*max*) 1000 (*n*) 25 (*m*)
suite 1000 (*iterations*) 100000 (*max*) 1000 (*n*) 25 (*m*)

