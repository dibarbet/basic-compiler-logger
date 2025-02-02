﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.CompilerLog.Util;

internal static class Extensions
{
    internal static ZipArchiveEntry GetEntryOrThrow(this ZipArchive zipArchive, string name)
    {
        var entry = zipArchive.GetEntry(name);
        if (entry is null)
            throw new InvalidOperationException($"Could not find entry with name {name}");
        return entry;
    }

    internal static Stream OpenEntryOrThrow(this ZipArchive zipArchive, string name)
    {
        var entry = GetEntryOrThrow(zipArchive, name);
        return entry.Open();
    }

    internal static string ReadLineOrThrow(this StreamReader reader)
    {
        if (reader.ReadLine() is { } line)
        {
            return line;
        }

        throw new InvalidOperationException();
    }

    internal static void AddRange<T>(this List<T> list, ReadOnlySpan<T> span)
    {
        foreach (var item in span)
        {
            list.Add(item);
        }
    }

    internal static List<byte> ReadAllBytes(this Stream stream)
    {
        var list = new List<byte>();
        Span<byte> buffer = stackalloc byte[256];
        do
        {
            var length = stream.Read(buffer);
            if (length == 0)
                break;

            list.AddRange(buffer.Slice(0, length));
        } while (true);

        return list;
    }
}
