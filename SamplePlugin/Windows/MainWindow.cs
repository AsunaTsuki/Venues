using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Numerics;
using System.Threading.Tasks;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using FFXIVVenues.VenueModels;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace SamplePlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private IDalamudTextureWrap? GoatImage;
    private Plugin Plugin;
    private readonly Task<Venue[]?> _venuesTask;
    public Venue[] venues = [];


    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin, IDalamudTextureWrap? goatImage,HttpClient httpClient)
        : base("My Amazing Window##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        GoatImage = goatImage;
        Plugin = plugin;

        this._venuesTask = httpClient.GetFromJsonAsync<Venue[]>("https://api.ffxivvenues.com/venue");
        venues = _venuesTask.Result;
        


}

    public void Dispose() { }

    public override void Draw()
    {
        
        foreach(var venue in this.venues)
        {
            if (ImGui.BeginChild("venueScrollArea", new Vector2(0, 400), true))
            {
                if (venue == null || string.IsNullOrEmpty(venue.BannerUri.ToString()))
                    continue;
                //if (ImGui.ImageButton(textureId, new Vector2(200, 100)))
                if (ImGui.Button(venue.Name))
                {
                    
                }


                /*// Retrieve the texture synchronously from preloaded textures
                var texture = _venueService.GetTexture(venue.BannerUri.ToString());
                var textureId = texture?.ImGuiHandle ?? IntPtr.Zero;

                if (textureId != IntPtr.Zero)
                {
                    if (ImGui.ImageButton(textureId, new Vector2(200, 100)))
                    {
                        Console.WriteLine($"Clicked on {venue.Name}");
                    }
                    ImGui.SameLine();
                    ImGui.Text(venue.Name);
                }
                else
                {
                    // Display a placeholder or loading text
                    ImGui.Text($"Loading {venue.Name}...");
                }*/
            }
            ImGui.EndChild();


        }

        if (GoatImage != null)
        {
            ImGuiHelpers.ScaledIndent(55f);
            ImGui.Image(GoatImage.ImGuiHandle, new Vector2(GoatImage.Width, GoatImage.Height));
            ImGuiHelpers.ScaledIndent(-55f);
        }
        else
        {
            ImGui.Text("Image not found.");
        }
    }
}
