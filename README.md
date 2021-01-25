# HASS Iconset Generator for Font-Awesome

This is a dotnet 5.0 program that generates iconsets compatible with Home Assistant after 0.110 (https://developers.home-assistant.io/blog/2020/05/09/custom-iconsets/). It is designed to support Font Awesome icons (V5 and V6), but should be easily extendable to other SVG files.

Only tested in Linux, but should work on other platforms.

## How to use
If you are looking for free/open iconsets check the `js` folder first. This project already has generated files for a few of those.

For all other iconsets, the easiest way to use this app is to use one of the already provided configuration files.
- v5-free.conf.json: Free V5 version of FontAwesome
- v5-pro.conf.json: Pro V5 version of FontAwesome
- v6-pro.conf.json: Pro V6 (currently alpha) version of FontAwesome
- simpleicons.conf.json: Configuration for the [simpleicons](https://github.com/simple-icons/simple-icons) project.

### Steps
1. Make sure you have dotnet core 5.0 installed https://dot.net
2. Download the Desktop version of FontAwesome (free or pro).
3. Unpack the SVG folder in `icons/v{5|6}-{free|pro}`
4. Run `dotnet run v{5|6}-{free|pro}.conf.json`
5. Generated files will be inside the `js` folder

### Format of config files:
To process other svg iconsets, a custom config file can be used:

```
{
    "outputPath": "js/v5-pro", // Where should the generated JS files be written.
    "iconsets": [ // An Array of iconsets to generate.
        {
            "directory": "icons/v5-pro/brands", // The path in which the .svg files can be found
            "name": "fab" // The name of the generated iconset. Used both for the js file and the ha prefix
        }
    ]
}
```

### How to install in Home Assistant
1. Add the generated files to the config/www folder.
2. Reference them as Lovelace resources. URL: [http://{homeassistant-url}/config/lovelace/resources](http://{homeassistant-url}/config/lovelace/resources)
3. Done!

## Future Plans
I'm looking to port the HASS custom component by `thomas-loven` to add automatic support to HA. Pull requests welcome
