// Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace FSharp.Core.Unittests.FSharp_Core.Microsoft_FSharp_Collections.SeqProperties

open System
open System.Collections.Generic
open NUnit.Framework
open FsCheck
open FSharp.Core.Unittests.FSharp_Core.Microsoft_FSharp_Collections.Utils

module StableProperties =
    let sortByStable<'a when 'a : comparison> (xs : 'a []) =
        let indexed = xs |> Seq.indexed
        let sorted = indexed |> Seq.sortBy snd
        isStable sorted

    [<Test>]
    let ``Seq.sortBy is stable`` () =
        Check.QuickThrowOnFailure sortByStable<int>
        Check.QuickThrowOnFailure sortByStable<string>

    let sortWithStable<'a when 'a : comparison> (xs : 'a []) =
        let indexed = xs |> Seq.indexed |> Seq.toList
        let sorted = indexed |> Seq.sortWith (fun x y -> compare (snd x) (snd y))
        isStable sorted

    [<Test>]
    let ``Seq.sortWithStable is stable`` () =
        Check.QuickThrowOnFailure sortWithStable<int>
        Check.QuickThrowOnFailure sortWithStable<string>

    let distinctByStable<'a when 'a : comparison> (xs : 'a []) =
        let indexed = xs |> Seq.indexed
        let sorted = indexed |> Seq.distinctBy snd
        isStable sorted

    [<Test>]
    let ``Seq.distinctBy is stable`` () =
        Check.QuickThrowOnFailure distinctByStable<int>
        Check.QuickThrowOnFailure distinctByStable<string>

module UnionProperties =
    let compareUnionWithAppendAndDistinct<'a when 'a : equality> (xs : 'a []) (ys : 'a []) =
        let union = Seq.union xs ys |> Array.ofSeq
        let appendAndDistinct = Seq.append xs ys |> Seq.distinct |> Array.ofSeq
        union = appendAndDistinct

    [<Test>]
    let ``Seq.union is like Seq.append >> Seq.distinct`` () =
        Check.QuickThrowOnFailure compareUnionWithAppendAndDistinct<int>
        Check.QuickThrowOnFailure compareUnionWithAppendAndDistinct<string>

    let xsUnionXs<'a when 'a : equality> (xs : 'a []) =
        let union = Seq.union xs xs |> Array.ofSeq
        let distinct = Seq.distinct xs |> Array.ofSeq
        union = distinct

    [<Test>]
    let ``Seq.union xs xs is like Seq.distinct`` () =
        Check.QuickThrowOnFailure xsUnionXs<int>
        Check.QuickThrowOnFailure xsUnionXs<string>

    let xsUnionEmpty<'a when 'a : equality> (xs : 'a []) =
        let union = Seq.union xs [||] |> Array.ofSeq
        let distinct = Seq.distinct xs |> Array.ofSeq
        union = distinct

    [<Test>]
    let ``Seq.union xs [] is like Seq.distinct`` () =
        Check.QuickThrowOnFailure xsUnionXs<int>
        Check.QuickThrowOnFailure xsUnionXs<string>

module IntersectionProperties =
    let compareIntersectionWithFilterAndDistinct<'a when 'a : equality> (xs : 'a []) (ys : 'a []) =
        let intersection = Seq.intersection xs ys |> Array.ofSeq
        let filterAndDistinct = xs |> Seq.filter (fun x -> Seq.contains x ys) |> Seq.distinct |> Array.ofSeq
        intersection = filterAndDistinct

    [<Test>]
    let ``Seq.intersection is like Seq.filter >> Seq.distinct`` () =
        Check.QuickThrowOnFailure compareIntersectionWithFilterAndDistinct<int>
        Check.QuickThrowOnFailure compareIntersectionWithFilterAndDistinct<string>

    let xsIntersectionXs<'a when 'a : equality> (xs : 'a []) =
        let intersection = Seq.intersection xs xs |> Array.ofSeq
        let distinct = Seq.distinct xs |> Array.ofSeq
        intersection = distinct

    [<Test>]
    let ``Seq.intersection xs xs is like Seq.distinct`` () =
        Check.QuickThrowOnFailure xsIntersectionXs<int>
        Check.QuickThrowOnFailure xsIntersectionXs<string>

    let xsIntersectionEmpty<'a when 'a : equality> (xs : 'a []) =
        let intersection = Seq.intersection xs [||] |> Array.ofSeq
        intersection = [||]

    [<Test>]
    let ``Seq.intersection xs [] is empty`` () =
        Check.QuickThrowOnFailure xsIntersectionXs<int>
        Check.QuickThrowOnFailure xsIntersectionXs<string>
