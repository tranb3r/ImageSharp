// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using System.Numerics;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp.Tests.PixelFormats;

[Trait("Category", "PixelFormats")]
public class Bgra5551Tests
{
    /// <summary>
    /// Tests the equality operators for equality.
    /// </summary>
    [Fact]
    public void AreEqual()
    {
        Bgra5551 color1 = new(0.0f, 0.0f, 0.0f, 0.0f);
        Bgra5551 color2 = new(new Vector4(0.0f));
        Bgra5551 color3 = new(new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
        Bgra5551 color4 = new(1.0f, 0.0f, 0.0f, 1.0f);

        Assert.Equal(color1, color2);
        Assert.Equal(color3, color4);
    }

    /// <summary>
    /// Tests the equality operators for inequality.
    /// </summary>
    [Fact]
    public void AreNotEqual()
    {
        Bgra5551 color1 = new(0.0f, 0.0f, 0.0f, 0.0f);
        Bgra5551 color2 = new(new Vector4(1.0f));
        Bgra5551 color3 = new(new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
        Bgra5551 color4 = new(1.0f, 1.0f, 0.0f, 1.0f);

        Assert.NotEqual(color1, color2);
        Assert.NotEqual(color3, color4);
    }

    [Fact]
    public void Bgra5551_PackedValue()
    {
        float x = 0x1a;
        float y = 0x16;
        float z = 0xd;
        float w = 0x1;
        Assert.Equal(0xeacd, new Bgra5551(x / 0x1f, y / 0x1f, z / 0x1f, w).PackedValue);
        Assert.Equal(3088, new Bgra5551(0.1f, -0.3f, 0.5f, -0.7f).PackedValue);

        Assert.Equal(0xFFFF, new Bgra5551(Vector4.One).PackedValue);
        Assert.Equal(0x7C00, new Bgra5551(Vector4.UnitX).PackedValue);
        Assert.Equal(0x03E0, new Bgra5551(Vector4.UnitY).PackedValue);
        Assert.Equal(0x001F, new Bgra5551(Vector4.UnitZ).PackedValue);
        Assert.Equal(0x8000, new Bgra5551(Vector4.UnitW).PackedValue);

        // Test the limits.
        Assert.Equal(0x0, new Bgra5551(Vector4.Zero).PackedValue);
        Assert.Equal(0xFFFF, new Bgra5551(Vector4.One).PackedValue);
    }

    [Fact]
    public void Bgra5551_ToVector4()
    {
        Assert.Equal(Vector4.Zero, new Bgra5551(Vector4.Zero).ToVector4());
        Assert.Equal(Vector4.One, new Bgra5551(Vector4.One).ToVector4());
    }

    [Fact]
    public void Bgra5551_ToScaledVector4()
    {
        // arrange
        Bgra5551 bgra = new(Vector4.One);

        // act
        Vector4 actual = bgra.ToScaledVector4();

        // assert
        Assert.Equal(1, actual.X);
        Assert.Equal(1, actual.Y);
        Assert.Equal(1, actual.Z);
        Assert.Equal(1, actual.W);
    }

    [Fact]
    public void Bgra5551_ToRgba32()
    {
        // arrange
        Bgra5551 bgra = new(Vector4.One);
        Rgba32 expected = new(Vector4.One);
        Rgba32 actual = default;

        // act
        bgra.ToRgba32(ref actual);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Bgra5551_FromScaledVector4()
    {
        // arrange
        Vector4 scaled = new Bgra5551(Vector4.One).ToScaledVector4();
        int expected = 0xFFFF;
        Bgra5551 pixel = default;

        // act
        pixel.FromScaledVector4(scaled);
        ushort actual = pixel.PackedValue;

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Bgra5551_FromBgra5551()
    {
        // arrange
        Bgra5551 bgra = default;
        Bgra5551 actual = default;
        Bgra5551 expected = new(1.0f, 0.0f, 1.0f, 1.0f);

        // act
        bgra.FromBgra5551(expected);
        actual.FromBgra5551(bgra);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Bgra5551_FromRgba32()
    {
        // arrange
        Bgra5551 bgra1 = default;
        Bgra5551 bgra2 = default;
        ushort expectedPackedValue1 = ushort.MaxValue;
        ushort expectedPackedValue2 = 0xFC1F;

        // act
        bgra1.FromRgba32(new Rgba32(255, 255, 255, 255));
        bgra2.FromRgba32(new Rgba32(255, 0, 255, 255));

        // assert
        Assert.Equal(expectedPackedValue1, bgra1.PackedValue);
        Assert.Equal(expectedPackedValue2, bgra2.PackedValue);
    }

    [Fact]
    public void Bgra5551_FromBgra32()
    {
        // arrange
        Bgra5551 bgra1 = default;
        Bgra5551 bgra2 = default;
        ushort expectedPackedValue1 = ushort.MaxValue;
        ushort expectedPackedValue2 = 0xFC1F;

        // act
        bgra1.FromBgra32(new Bgra32(255, 255, 255, 255));
        bgra2.FromBgra32(new Bgra32(255, 0, 255, 255));

        // assert
        Assert.Equal(expectedPackedValue1, bgra1.PackedValue);
        Assert.Equal(expectedPackedValue2, bgra2.PackedValue);
    }

    [Fact]
    public void Bgra5551_FromArgb32()
    {
        // arrange
        Bgra5551 bgra = default;
        ushort expectedPackedValue = ushort.MaxValue;

        // act
        bgra.FromArgb32(new Argb32(255, 255, 255, 255));

        // assert
        Assert.Equal(expectedPackedValue, bgra.PackedValue);
    }

    [Fact]
    public void Bgra5551_FromRgb48()
    {
        // arrange
        Bgra5551 bgra = default;
        ushort expectedPackedValue = ushort.MaxValue;

        // act
        bgra.FromRgb48(new Rgb48(ushort.MaxValue, ushort.MaxValue, ushort.MaxValue));

        // assert
        Assert.Equal(expectedPackedValue, bgra.PackedValue);
    }

    [Fact]
    public void Bgra5551_FromRgba64()
    {
        // arrange
        Bgra5551 bgra = default;
        ushort expectedPackedValue = ushort.MaxValue;

        // act
        bgra.FromRgba64(new Rgba64(ushort.MaxValue, ushort.MaxValue, ushort.MaxValue, ushort.MaxValue));

        // assert
        Assert.Equal(expectedPackedValue, bgra.PackedValue);
    }

    [Fact]
    public void Bgra5551_FromGrey16()
    {
        // arrange
        Bgra5551 bgra = default;
        ushort expectedPackedValue = ushort.MaxValue;

        // act
        bgra.FromL16(new L16(ushort.MaxValue));

        // assert
        Assert.Equal(expectedPackedValue, bgra.PackedValue);
    }

    [Fact]
    public void Bgra5551_FromGrey8()
    {
        // arrange
        Bgra5551 bgra = default;
        ushort expectedPackedValue = ushort.MaxValue;

        // act
        bgra.FromL8(new L8(byte.MaxValue));

        // assert
        Assert.Equal(expectedPackedValue, bgra.PackedValue);
    }

    [Fact]
    public void Bgra5551_FromBgr24()
    {
        // arrange
        Bgra5551 bgra = default;
        ushort expectedPackedValue = ushort.MaxValue;

        // act
        bgra.FromBgr24(new Bgr24(byte.MaxValue, byte.MaxValue, byte.MaxValue));

        // assert
        Assert.Equal(expectedPackedValue, bgra.PackedValue);
    }

    [Fact]
    public void Bgra5551_FromRgb24()
    {
        // arrange
        Bgra5551
            bgra = default;
        ushort expectedPackedValue = ushort.MaxValue;

        // act
        bgra.FromRgb24(new Rgb24(byte.MaxValue, byte.MaxValue, byte.MaxValue));

        // assert
        Assert.Equal(expectedPackedValue, bgra.PackedValue);
    }

    [Fact]
    public void Bgra5551_Clamping()
    {
        Assert.Equal(Vector4.Zero, new Bgra5551(Vector4.One * -1234.0f).ToVector4());
        Assert.Equal(Vector4.One, new Bgra5551(Vector4.One * 1234.0f).ToVector4());
    }

    [Fact]
    public void Bgra5551_PixelInformation()
    {
        PixelTypeInfo info = Bgra5551.GetPixelTypeInfo();
        Assert.Equal(Unsafe.SizeOf<Bgra5551>() * 8, info.BitsPerPixel);
        Assert.Equal(PixelAlphaRepresentation.Unassociated, info.AlphaRepresentation);
        Assert.Equal(PixelColorType.BGR | PixelColorType.Alpha, info.ColorType);

        PixelComponentInfo componentInfo = info.ComponentInfo.Value;
        Assert.Equal(4, componentInfo.ComponentCount);
        Assert.Equal(0, componentInfo.Padding);
        Assert.Equal(5, componentInfo.GetComponentPrecision(0));
        Assert.Equal(5, componentInfo.GetComponentPrecision(1));
        Assert.Equal(5, componentInfo.GetComponentPrecision(2));
        Assert.Equal(1, componentInfo.GetComponentPrecision(3));
        Assert.Equal(5, componentInfo.GetMaximumComponentPrecision());
    }
}
