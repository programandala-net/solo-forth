#! /usr/bin/env gforth

\ pbm2scr

: version  s" 0.3.1+201612302009"  ;

\ Last modified: 201612302009

\ ==============================================================
\ Description

\ pbm2scr is a command line tool that converts 256x192 PBM
\ graphics (P1 and P4 versions) to ZX Spectrum SCR graphic
\ files.

\ http://programandala.net/en.program.pbm2scr.html

\ This program is written in Forth (http://forth-standard.org)
\ with Gforth (http://gnu.org/software/gforth/).

\ ==============================================================
\ Author and license

\ Copyright (C) 2015,2016 Marcos Cruz (programandala.net)

\ You may do whatever you want with this work, so long as you
\ retain the copyright notice(s) and this license in all
\ redistributed copies and derived works. There is no warranty.

\ ==============================================================
\ Acknowledgements

\ The information on the PBM format was obtained from the manual
\ page of the Netpbm Debian package. But also the Wikipedia
\ provides a useful description of the format:

\   https://en.wikipedia.org/wiki/Netpbm_format

\ The following article was most useful to code the low level
\ calculation of the ZX Spectrum screen addresses:

\   "Cómo manejar la pantalla desde código máquina" by Paco
\   Portalo, published in Microhobby (issue 63, 1986-02, page
\   30):
\     http://www.microhobby.org/numero063.htm
\     http://microhobby.speccy.cz/mhf/063/MH063_30.jpg
\     http://microhobby.speccy.cz/mhf/063/MH063_31.jpg

\ ==============================================================
\ Installation

\ 1) Install Gforth. It's included in most Linux distros, or you
\ can get it from <http://gnu.org/software/gforth>.
\
\ 2) Make sure <pbm2scr.fs> is executable:
\
\   chmod 0755 pbm2scr.fs
\
\ 3) Link <pbm2scr.fs> to </usr/local/bin/>, with the filename
\ extension removed:
\
\   ln pbm2scr.fs /usr/local/bin/pbm2scr
\
\ Depending on your system configuration, you may need a
\ symbolic link instead, or a different target directory.

\ ==============================================================
\ History

\ See at the end of the file.

\ ==============================================================
\ Requirements

forth definitions

\ ----------------------------------------------
\ From the Galope library
\ (http://programandala.net/en.program.galope.html)

: unslurp-file  ( ca1 len1 ca2 len2 -- )
  w/o create-file throw >r
  r@ write-file throw
  r> close-file throw  ;
  \ Save string _ca1 len1_ to file _ca2 len2_.

: -bounds  ( ca1 len -- ca2 ca1 )
  2dup + 1- nip  ;
  \ Convert an address and length to the parameters needed by a
  \ "do ... -1 +loop" in order to examine that memory zone in
  \ reverse order.

: -extension  ( ca1 len1 -- ca1 len1' )
  2dup -bounds 1+ 2swap  \ default raw return values
  -bounds ?do
    i c@ '.' = if  drop i  leave  then
  -1 +loop  ( ca1 ca1' )  \ final raw return values
  over -  ;
  \ Remove the file extension of a filename.

\ ==============================================================
\ ZX Spectrum screen

\ A ZX Spectrum screen has two parts: first, the bitmap: 6144
\ bytes that represent a 256x192 bitmap, with a special order;
\ second, the attributes: 768 bytes (32x24 character positions)
\ that describe the colors of every 8x8 square of the bitmap.

6144 constant /zxscr-bitmap
 768 constant /zxscr-attributes
/zxscr-bitmap /zxscr-attributes + constant /zxscr

/zxscr allocate throw constant zxscr

zxscr /zxscr-bitmap + constant zxscr-attributes

%00111000 constant color  \ default color: white paper, black ink
  \ bits 0..2 = ink
  \ bits 3..5 = paper
  \ bit 6     = bright
  \ bit 7     = flash

: init-zxscr  ( -- )
  zxscr /zxscr-bitmap erase
  zxscr-attributes /zxscr-attributes color fill  ;
  \ Init the ZX Spectrum screen buffer.

\ ==============================================================
\ Bitmap address converter

\ The ZX Spectrum screen bitmap is arranged in a very special way.
\ Bit-level calculations are needed to calculate the destination
\ address of every byte of the input bitmap.

 32 constant chars/line                 \ chars per line
192 constant heigth                     \ pixels
/zxscr-bitmap 3 / constant /zxscr-third \ bytes per bitmap third
256 constant chars/third                \ chars per third

variable third        \ third of the bitmap (0..2)
variable char/third   \ character in the third (0..255)
variable char-row     \ character row (0..23)
variable char-col     \ character row (0..31)
variable char-scan    \ character scan (0..7)
variable pixel-row    \ pixel row (0..191); top row is 0

: pbm-index>data  ( +n -- )
  dup /zxscr-third / third !                \ bitmap third (0..2)
  dup chars/line /                          \ y row (0..191)
      dup %000111 and char-scan !           \ char scan (0..7)
          %111000 and 3 rshift pixel-row !  \ pixel row (0..7)
      chars/line mod char-col !  ;          \ char col (0..31)
  \ Convert the input position _+n_ to the data required
  \ for further calculation.

: data>scr-index  ( -- +n )
  pixel-row @ 32 *  char-col @ +  \ low byte
  third @ 8 * char-scan @ +       \ high byte
  256 * +  ;                      \ result
  \ Calculate the output position _+n_ in the ZX Spectrum bitmap
  \ (0..6143).

: >zxscr  ( +n -- ca )
  pbm-index>data data>scr-index
  dup /zxscr > abort" Bitmap bigger than 256x192"
  zxscr +  ;  \ actual address in the output buffer
  \ Convert a position in the input file PBM bitmap (0..6143)
  \ to its correspondent address in the SCR bitmap buffer.

\ ==============================================================
\ Bitmap byte converter

\ There are two variants of the PBM format: binary (P4
\ identifier) and ASCII (P1 identifier).
\
\ The bitmap bytes of the binary variant are ready to be copied
\ into the ZX Spectrum bitmap, though in different positions.
\
\ The bitmap bytes of the ASCII variant are represented by eight
\ characters, so first they have to be calculated.

: byte  ( -- b )
  source-id key-file  ;
  \ Get the next byte from the input file, currently being
  \ interpreted as a Forth source file.

: delimiter?  ( c -- f )
  bl <=  ;
  \ Is the given char a delimiter in the P1 bitmap?

: p1-bit-char  ( -- c )
  begin   byte dup delimiter?  while  drop  repeat  ;
  \ Get the next bit from the P1 input file,
  \ represented by an ASCII character:
  \ "1"=black; "0"=white.

: p1-pixel?  ( -- f )
  p1-bit-char [char] 1 =  ;
  \ Get the next pixel from the P1 input file.
  \ Return _true_ if black pixel; _false_ if white pixel.

: p1-byte  ( -- b )
  0  \ return value
  8 0 do  p1-pixel? 128 and i rshift or  loop  ;
  \ Get the next bitmap byte from the P1 input file,
  \ represented by 8 ASCII characters:
  \ "1"=black; "0"=white.

defer bitmap-byte  ( -- b )
  ' false is bitmap-byte  \ default, used for checking
  \ Get the next bitmap byte from the input file.

: bitmap>scr  ( "<bitmap>" -- )
  /zxscr-bitmap 0 do  bitmap-byte i >zxscr c!  loop  ;
  \ Get the PBM bitmap and convert it to the SCR bitmap.

\ ==============================================================
\ PBM header interpreter

\ The header of a PBM file is ASCII, and their elements are
\ separated by ordinary text delimiters.
\
\ The chosen approach is to define Forth words with the name of
\ the expected header metadata, so the PBM file can be simply
\ interpreted as a Forth source file.

variable width?
  \ Flag: has the width been found in the input file?

: check-type  ( -- )
  ['] bitmap-byte defer@ ['] false =  \ `bitmap-byte` not set?
  abort" File type not supported"  ;
  \ Abort if no file type was specified in the file header.

: check-width  ( -- )
  width? @ 0= abort" The bitmap size must be 256x192"  ;
  \ Abort if the width was not found in the file header.

wordlist constant pbm-wordlist  \ words allowed in the PBM file
pbm-wordlist set-current

\ Only five words are needed to interpret a 256x192 PBM file:
\ the two possible magic numbers, the line comment character,
\ the width and the heigth. The heigth is the last one in the
\ file header and it will do the conversion of the bitmap.

: p1  ( -- )
  ['] p1-byte is bitmap-byte  ;
  \ P1 PBM, the ASCII variant of the format.

: p4  ( -- )
  ['] byte is bitmap-byte  ;
  \ P4 PBM, the binary variant of the format.

' \ alias #  ( "ccc<newline>" -- )
  \ Line comment.

: 256  ( -- )
  check-type  width? on  ;
  \ The width of the image.
  \ This is the last but one metadata before the bitmap.

: 192  ( -- )
  check-type check-width  bitmap>scr  ;
  \ The heigth of the image.
  \ This is the last metadata before the bitmap.
  \ If everything is ok, get the bitmap.

\ ==============================================================
\ File converter

forth definitions

: working-dir  ( -- ca len )
  s" PWD" getenv  ;
  \ Current working directory.

: working-dir+  ( ca1 len1 -- ca2 len2 )
  working-dir s" /" s+ 2swap s+  ;
  \ Add the current working directory to a file name.

: save-scr  ( ca len -- )
  -extension s" .scr" s+ zxscr /zxscr 2swap unslurp-file  ;
  \ Save the SCR buffer to the output file,
  \ after the input file name _ca len_.

: pbm>scr  ( ca len -- )
  2>r  get-order
  init-zxscr pbm-wordlist >order seal
  2r@ working-dir+ included
  set-order  2r> save-scr  ;
  \ Convert a 256x192 PBM file _ca len_ to a ZX Spectrum SCR
  \ file.

: about  ( -- )
  ." pbm2scr" cr
  ." PBM to ZX Spectrum SCR graphic converter" cr
  ." Version " version type cr
  ." http://programandala.net/en.program.pbm2scr.html" cr cr
  ." Copyright (C) 2015,2016 Marcos Cruz (programandala.net)" cr cr
  ." Usage:" cr
  ."   pbm2scr [INPUT-FILES]" cr cr
  ." The input file must be a 256x192 PBM image," cr
  ." in binary or ASCII variants of the format." cr cr
  ." The output file name will be the input file name" cr
  ." with the '.scr' extension instead of '.pbm'." cr  ;

: input-files  ( -- n )
  argc @ 1-  ;
  \ Number of input files in the command line.

: run  ( -- )
  input-files ?dup
  if    0 do  i 1+ arg pbm>scr  loop
  else  about  then  ;
  \ Convert all input PBM files to ZX Spectrum SCR files.

run bye

\ ==============================================================
\ History

\ 2015-04-26: Start. Version A-00.
\
\ 2015-04-27: Version A-01, first working version: it converts
\ P1 and P4 variants of the PBM format.  Version A-02: it
\ accepts any number of input files in the command line.
\
\ 2015-04-28: The extension of the output file name is not
\ simply appended any more but substitutes that of the input
\ file name.
\
\ 2015-08-21: Created constant for the default color. Removed
\ the debugging code. Added installation instructions (the
\ Galope library is pending). Improved the text in `about`.
\
\ 2015-12-12: Started changing the code style, mainly the
\ position of comments, in order to publish it. Factored
\ `>zxscr` into three words.  Changed the version numbering
\ system. Version 0.3.0. Added documentation and samples.
\
\ 2016-11-19: Fix the format of the version number, after
\ Semantic Versioning (http://semver.org). Add direct links to
\ Microhobby pages.
\
\ 2016-11-20: Fix comment.
\
\ 2016-12-30: Modify the header comments.

\ vim: textwidth=64

