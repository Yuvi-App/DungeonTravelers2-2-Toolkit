# DungeonTravelers2-2-Toolkit

Created this to work on a potential port of the DT2-2 PC English version to the Japanese VITA version. 

The games are more different than one would expect unfortunately so it's not a straightforward port.

While it's not impossible there are some challenges, I might revisit it when I have some more time. But wanted to document some issues as of right now.

## What does the Tool Do?
- Extract PCK's
- Creates PCK's
- Extract Images from .TEX (PC or VITA Compatible, as they are two different formats)
- Create new .TEX from .PNG (PC or VITA Compatible)
- Converts & resizes PC .TEX files into VITA compatible ones.


## Challenges
- Images are not aligned the same. For example, the title screen is laid out completely differently from one another, which means you will need to manually align all the images with proper graphics to make it work on VITA.
  PC Images also contain more information than what was in VITA, however, all the images are there in English to make the port happen, just have to fix the alignment. PC Images are also way bigger/higher resolution than the VITA, so all images will need to be downsampled as well to fit the VITA. As you are unable to inject high-resolution images, as the game will just not render correctly. (trust me I tried)
- The game file structure is slightly different. The VITA version "surprisingly" used a lot more uncompressed files vs the PC version. Most of the PC files are in LZ77, which we can decrypt and then use for the VITA. The PC version is also structured slightly       
  different, with a lot of folder structure just being compressed into a PCK file, while the VITA version leaves all of that in a normal folder structure.
- Thankfully most of the script files and stuff with dialog are laid out exactly the same, so you can actually inject those into the VITA version with little work.
- The Real Killer FONT/Text engine. So the two versions are completely different. Not only does the PC version contain over 8 different .fnt, but they are also compressed into a PCK, meaning they are incompatible with the VITA version in general due to this. The VITA version is just .fnt files. However, those are really just PNG files that contain all the proper glyphs. The good news is the font contains everything you could want. The bad news is the way the game renders font is really wack. For example, in-game cutscenes, the game will refuse to load half-width strings at all, so you are stuck with full-width text. However in the menus/battle, the game has no issue displaying half-width text, however, it still takes up too much width. I've gone in and out of the game using other font files and trying to locate the textbox control function for the game but no dice on anything as of yet.
  
## File Format
- .tex - Texture file, 0x20 header with image info. Then either LZ77 compressed PNG or just uncompressed PNG, then the end of the file contains palette information.
        This TEX format is different for nearly every Aquaplus game, and unfortunately, the VITA and PC version of DT2-2 uses two different versions of this format. This tool has a convertor, that will take a PC image, and make it work for the VITA.
- .pck - Pretty common aquaplus archive format, these are used throughout.

  Some additional info here https://x.com/YuviApp/status/1717375312319631811?s=20
