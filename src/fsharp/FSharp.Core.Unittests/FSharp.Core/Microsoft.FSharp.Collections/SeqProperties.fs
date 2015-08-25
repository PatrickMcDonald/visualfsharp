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
        let actual = Seq.union ys xs
        let expected = Seq.append xs ys |> Seq.distinct
        seqsAreEqual expected actual

    [<Test>]
    let ``Seq.union is like Seq.append >> Seq.distinct`` () =
        Check.QuickThrowOnFailure compareUnionWithAppendAndDistinct<int>
        Check.QuickThrowOnFailure compareUnionWithAppendAndDistinct<string>

    let xsUnionXs<'a when 'a : equality> (xs : 'a []) =
        let actual = Seq.union xs xs
        let expected = Seq.distinct xs
        seqsAreEqual expected actual

    [<Test>]
    let ``Seq.union xs xs is like Seq.distinct`` () =
        Check.QuickThrowOnFailure xsUnionXs<int>
        Check.QuickThrowOnFailure xsUnionXs<string>

    let xsUnionEmpty<'a when 'a : equality> (xs : 'a []) =
        let expected = Seq.distinct xs
        seqsAreEqual expected (Seq.union xs []) && seqsAreEqual expected (Seq.union [] xs)

    [<Test>]
    let ``Seq.union xs [] is like Seq.distinct`` () =
        Check.QuickThrowOnFailure xsUnionEmpty<int>
        Check.QuickThrowOnFailure xsUnionEmpty<string>

module IntersectionProperties =
    let compareIntersectionWithFilterAndDistinct<'a when 'a : equality> (xs : 'a []) (ys : 'a []) =
        let actual = Seq.intersection ys xs
        let expected = xs |> Seq.filter (fun x -> Seq.contains x ys) |> Seq.distinct
        seqsAreEqual expected actual

    [<Test>]
    let ``Seq.intersection is like Seq.filter >> Seq.distinct`` () =
        Check.QuickThrowOnFailure compareIntersectionWithFilterAndDistinct<int>
        Check.QuickThrowOnFailure compareIntersectionWithFilterAndDistinct<string>

    let xsIntersectionXs<'a when 'a : equality> (xs : 'a []) =
        let actual = Seq.intersection xs xs
        let expected = Seq.distinct xs
        seqsAreEqual expected actual

    [<Test>]
    let ``Seq.intersection xs xs is like Seq.distinct`` () =
        Check.QuickThrowOnFailure xsIntersectionXs<int>
        Check.QuickThrowOnFailure xsIntersectionXs<string>

    let xsIntersectionEmpty<'a when 'a : equality> (xs : 'a []) =
        let expected = Seq.empty
        seqsAreEqual expected (Seq.intersection xs []) && seqsAreEqual expected (Seq.intersection [] xs)

    [<Test>]
    let ``Seq.intersection xs [] is empty`` () =
        Check.QuickThrowOnFailure xsIntersectionEmpty<int>
        Check.QuickThrowOnFailure xsIntersectionEmpty<string>
