// Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace FSharp.Core.Unittests.FSharp_Core.Microsoft_FSharp_Collections.ArrayProperties

open System
open System.Collections.Generic
open NUnit.Framework
open FsCheck

module StableProperties =
    let isStable sorted = sorted |> Seq.pairwise |> Seq.forall (fun ((ia, a),(ib, b)) -> if a = b then ia < ib else true)

    let distinctByStable<'a when 'a : comparison> (xs : 'a []) =
        let indexed = xs |> Seq.indexed |> Seq.toArray
        let sorted = indexed |> Array.distinctBy snd
        isStable sorted

    [<Test>]
    let ``Seq.distinctBy is stable`` () =
        Check.QuickThrowOnFailure distinctByStable<int>
        Check.QuickThrowOnFailure distinctByStable<string>

module UnionProperties =
    let compareUnionWithAppendAndDistinct<'a when 'a : equality> (xs : 'a []) (ys : 'a []) =
        let actual = Array.union ys xs
        let expected = Array.append xs ys |> Array.distinct
        expected = actual

    [<Test>]
    let ``Array.union is like Array.append >> Array.distinct`` () =
        Check.QuickThrowOnFailure compareUnionWithAppendAndDistinct<int>
        Check.QuickThrowOnFailure compareUnionWithAppendAndDistinct<string>

    let xsUnionXs<'a when 'a : equality> (xs : 'a []) =
        let actual = Array.union xs xs
        let expected = Array.distinct xs
        expected = actual

    [<Test>]
    let ``Array.union xs xs is like Array.distinct`` () =
        Check.QuickThrowOnFailure xsUnionXs<int>
        Check.QuickThrowOnFailure xsUnionXs<string>

    let xsUnionEmpty<'a when 'a : equality> (xs : 'a []) =
        let expected = Array.distinct xs
        expected = (Array.union xs [||]) && expected = (Array.union [||] xs)

    [<Test>]
    let ``Array.union xs [||] is like Array.distinct`` () =
        Check.QuickThrowOnFailure xsUnionEmpty<int>
        Check.QuickThrowOnFailure xsUnionEmpty<string>

module IntersectionProperties =
    let compareIntersectionWithFilterAndDistinct<'a when 'a : equality> (xs : 'a []) (ys : 'a []) =
        let actual = Array.intersection ys xs
        let expected = xs |> Array.filter (fun x -> Array.contains x ys) |> Array.distinct
        expected = actual

    [<Test>]
    let ``Array.intersection is like Array.filter >> Array.distinct`` () =
        Check.QuickThrowOnFailure compareIntersectionWithFilterAndDistinct<int>
        Check.QuickThrowOnFailure compareIntersectionWithFilterAndDistinct<string>

    let xsIntersectionXs<'a when 'a : equality> (xs : 'a []) =
        let actual = Array.intersection xs xs
        let expected = Array.distinct xs
        expected = actual

    [<Test>]
    let ``Array.intersection xs xs is like Array.distinct`` () =
        Check.QuickThrowOnFailure xsIntersectionXs<int>
        Check.QuickThrowOnFailure xsIntersectionXs<string>

    let xsIntersectionEmpty<'a when 'a : equality> (xs : 'a []) =
        [||] = (Array.intersection xs [||]) && [||] = (Array.intersection [||] xs)

    [<Test>]
    let ``Array.intersection xs [||] is empty`` () =
        Check.QuickThrowOnFailure xsIntersectionEmpty<int>
        Check.QuickThrowOnFailure xsIntersectionEmpty<string>
