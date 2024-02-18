using System.Collections.ObjectModel;
using Logo2Svg.SVG;

namespace Logo2Svg.Turtle;

public static class ColourPalette
{
    public static readonly Colour[] Palette =
    {
        new(0,   0,   0),   // black
        new(0,   0,   255), // blue
        new(0,   255, 0),   // green
        new(0,   255, 255), // cyan
        new(255, 0,   0),   // red
        new(255, 0,   255), // magenta
        new(255, 255, 0),   // yellow
        new(255, 255, 255), // white
        new(165, 42,  42),  // brown
        new(210, 180, 140), // tan
        new(0,   128, 0),   // green
        new(127, 255, 212), // aqua
        new(250, 128, 114), // salmon
        new(128, 0,   128), // purple
        new(255, 165, 0),   // orange
        new(128, 128, 128)  // gray
    };

    public static readonly ReadOnlyDictionary<string, Colour> ColourNames =
        new(new Dictionary<string, Colour> {
            {"black",   new Colour(0,   0,   0)},
            {"silver",  new Colour(192, 192, 192)},
            {"gray",    new Colour(128, 128, 128)},
            {"white",   new Colour(255, 255, 255)},
            {"maroon",  new Colour(128, 0,   0)},
            {"red",     new Colour(255, 0,   0)},
            {"purple",  new Colour(128, 0,   128)},
            {"fuchsia", new Colour(255, 0,   255)},
            {"green",   new Colour(0,   128, 0)},
            {"lime",    new Colour(0,   255, 0)},
            {"olive",   new Colour(128, 128, 0)},
            {"yellow",  new Colour(255, 255, 0)},
            {"navy",    new Colour(0,   0,   128)},
            {"blue",    new Colour(0,   0,   255)},
            {"teal",    new Colour(0,   128, 128)},
            {"aqua",    new Colour(0,   255, 255)}
        });
}