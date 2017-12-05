  \ tape.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201707262116
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Tape files support. The only supported filetype is "code".
  \ Contrary to BASIC, saving starts immediately (the message
  \ "Start tape, then press any key" is not printed).
  \
  \ Known issues:
  \
  \ 1) If the space key is pressed while reading or writing
  \ files, the ROM routine will issue a BASIC error and make
  \ the system crash.  This may be solved in the future, with
  \ the help of G+DOS, by trapping the error.
  \
  \ 2) Tape loading errors are not trapped. They make the
  \ system crash.
  \
  \ 3) No support to verify saved files. It may be added in the
  \ future, though it's not useful with emulators, without
  \ actual tapes.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ Credit

  \ The required information about the tape routines was
  \ extracted from from Don Thomasson's book _Advanced Spectrum
  \ Forth_ (page 119), the ZX Spectrum ROM disassembly (whose
  \ description of the tape headers is wrong), the _Abersoft
  \ Forth disassembled_ project
  \ (http://programandala.net/en.program.abersoft_forth.html)
  \ and the Afera library
  \ (http://programandala.net/en.program.afera.html).

( tape-header )

17 cconstant /tape-header

  \ doc{
  \
  \ /tape-header ( -- n )
  \
  \ _n_ is the length of a `tape-header`: 17 bytes.
  \
  \ }doc

create tape-header  /tape-header 2 * allot

  \ doc{
  \
  \ tape-header ( -- a )
  \
  \ Address of the tape header, which is used by the ROM
  \ routines. Its structure is the following:

  \ .Structure of a tape header
  \ |===
  \ | Offset  | Size     | Description
  \
  \ | +00     | byte     | filetype (3 for code files)
  \ | +01     | 10 chars | filename, padded with spaces
  \ | +11     | cell     | length
  \ | +13     | cell     | start address
  \ | +15     | cell     | not used for code files
  \ |===

  \ When the first char of the filename is 255, it is regarded
  \ as a wildcard which will match any filename. The word
  \ `tape-file>` sets the wildcard when the provided filename
  \ is empty.
  \
  \ A second tape header, pointed by `last-tape-header`,
  \ follows the main one. It is used by the ROM routines while
  \ loading. It can be used by the application to know the
  \ details of the last tape file that was loaded.
  \
  \ IX addresses the first header, which must contain the data.
  \ The second header is used by the system when loading and
  \ verifying. Only the "CODE" file type column is relevant to
  \ Solo Forth.

  \ .Detailed structure of both tape headers
  \ |===
  \ | First header | Second header | BASIC program | Num DATA | String DATA | CODE | Notes

  \ | IX+$00 | IX+$11 | 0     | 1   | 2   | 3     | File type
  \ | IX+$01 | IX+$12 | x     | x   | x   | x     | F ($FF if filename is null)
  \ | IX+$02 | IX+$13 | x     | x   | x   | x     | i
  \ | IX+$03 | IX+$14 | x     | x   | x   | x     | l
  \ | IX+$04 | IX+$15 | x     | x   | x   | x     | e
  \ | IX+$05 | IX+$16 | x     | x   | x   | x     | n
  \ | IX+$06 | IX+$17 | x     | x   | x   | x     | a
  \ | IX+$07 | IX+$18 | x     | x   | x   | x     | m
  \ | IX+$08 | IX+$19 | x     | x   | x   | x     | e
  \ | IX+$09 | IX+$1A | x     | x   | x   | x     | .
  \ | IX+$0A | IX+$1B | x     | x   | x   | x     | Padding spaces
  \ | IX+$0B | IX+$1C | lo    | lo  | lo  | lo    | Total...
  \ | IX+$0C | IX+$1D | hi    | hi  | hi  | hi    | ...length of datablock
  \ | IX+$0D | IX+$1E | Auto  | -   | -   | Start | Various
  \ | IX+$0E | IX+$1F | Start | a-z | a-z | addr  | ($80 if no autostart).
  \ | IX+$0F | IX+$20 | lo    | -   | -   | -     | Length of program only...
  \ | IX+$10 | IX+$21 | hi    | -   | -   | -     | ...i.e. without variables
  \ |===

  \ See: `tape-filename`, `tape-filetype`, `tape-start`,
  \ `tape-length`, `any-tape-filename`, `?set-tape-filename`.
  \
  \ }doc

10 cconstant /tape-filename \ filename max length

  \ doc{
  \
  \ /tape-filename ( -- n )
  \
  \ _n_ is the maximum length of a tape filename, which is 10
  \ characters.
  \
  \ See: `tape-filename`. `/filename`.
  \
  \ }doc

tape-header constant tape-filetype ( -- ca ) 3 tape-filetype c!

  \ doc{
  \
  \ tape-filetype ( -- ca )
  \
  \ Address of the file type (one byte) in `tape-header`.
  \ Its default value is 3 (code file).
  \
  \ See: `last-tape-filetype`.
  \
  \ }doc

tape-header 1+ constant tape-filename ( -- ca )

  \ doc{
  \
  \ tape-filename ( -- ca )
  \
  \ Address of the filename in `tape-header`.
  \
  \ See: `/tape-filename`, `set-tape-filename`,
  \ `last-tape-filename`.
  \
  \ }doc

tape-header 11 + constant tape-length ( -- a )

  \ doc{
  \
  \ tape-length ( -- a )
  \
  \ Address of the file length in `tape-header`.
  \
  \ See: `last-tape-length`.
  \
  \ }doc

tape-header 13 + constant tape-start ( -- a )

  \ doc{
  \
  \ tape-start ( -- a )
  \
  \ Address of the file start in `tape-header`.
  \
  \ See: `last-tape-start`.
  \
  \ }doc

: -tape-filename ( -- ) tape-filename /tape-filename blank ;

  \ doc{
  \
  \ -tape-filename ( -- )
  \
  \ Blank `tape-filename` in `tape-header`.
  \
  \ }doc

: any-tape-filename ( -- ) 255 tape-filename c! ;

  \ doc{
  \
  \ any-tape-filename ( -- )
  \
  \ Configure `tape-header` to load any filename, by replacing
  \ the first char of `tape-filename` with 255, which will be
  \ recognized as a wild card.
  \
  \ }doc

: set-tape-filename ( ca len -- )
  -tape-filename  /tape-filename min tape-filename swap cmove ;

  \ doc{
  \
  \ set-tape-filename ( ca len -- )
  \
  \ Store filename _ca len_ into the `tape-filename` field of
  \ `tape-header`.
  \
  \ }doc

: ?set-tape-filename ( ca len -- )
  dup if    set-tape-filename
      else  2drop any-tape-filename  then ;

  \ doc{
  \
  \ ?set-tape-filename ( ca len -- )
  \
  \ If filename _ca len_ is not empty, store it into the tape
  \ header by executing `set-tape-filename`; else use a
  \ wildcard instead, by executing `any-tape-filename`.
  \
  \ }doc

: set-tape-memory ( ca len -- ) tape-length ! tape-start ! ;

  \ doc{
  \
  \ set-tape-memory ( ca len -- )
  \
  \ Configure `tape-header` with the memory zone _ca len_ (to
  \ be read or written), by storing _len_ into `tape-length`
  \ and _ca_ into `tape-start`.
  \
  \ }doc

( tape-file> >tape-file )

[unneeded] tape-file> ?( need tape-header

code (tape-file>) ( -- )
  C5 c,  DD c, 21 c, tape-header ,  2A c, tape-start ,
    \ push bc ; save Forth IP
    \ ld ix,tape_header
    \ ld hl,(tape_start)
  3E c, 01 c,  32 c, 5C74 ,  CD c, 075A ,
    \ ld a,1 ; 1=load
    \ ld (5C74),A ; T_ADDR system variable
    \ call 075A ; SA_ALL ROM routine
  C1 c,  DD c, 21 c, next , jpnext, end-code
    \ pop bc ; restore Forth IP
    \ ld ix,next ; restore the address of Forth `next`

  \ doc{
  \
  \ (tape-file>) ( -- )
  \
  \ Read a tape file using the data stored at `tape-header`.
  \
  \ ``(tape-file>)`` is a factor of `tape-file>`.
  \
  \ }doc

: tape-file> ( ca1 len1 ca2 len2 -- )
  set-tape-memory ?set-tape-filename (tape-file>) ; ?)

  \ doc{
  \
  \ tape-file> ( ca1 len1 ca2 len2 -- )
  \
  \ Read a tape file _ca1 len1_ (_len1_ is zero if filename is
  \ unspecified) into a memory region _ca2 len2_.
  \
  \ _ca2_ is zero if the address must be taken from the file
  \ header instead, which is the address the file was saved
  \ from.  _len2_ is zero if it's unspecified.
  \
  \ WARNING: If _len2_ is not zero or the exact length of the
  \ file, the ROM routine returns to BASIC with "Tape loading
  \ error". This crashes the system, because in Solo Forth the
  \ lower screen has no lines, and BASIC can not display the
  \ message. This will be avoided in a future version of Solo
  \ Forth.
  \
  \ See: `>tape-file`, `(tape-file>)`, `>file`.
  \
  \ }doc

[unneeded] >tape-file ?( need tape-header

code (>tape-file) ( -- )
  C5 c,  DD c, 21 c, tape-header , A8 07 + c,  32 c, 5C74 ,
    \ push bc               ; save Forth IP
    \ ld ix,tape_header
    \ xor a                 ; 0=save
    \ ld (5C74),a           ; T_ADDR system variable
  21 c, here 0A + ,  E5 c, 2A c, tape-start ,  E5 c,
    \ ld hl,return_from_ROM
    \ push hl               ; simulate a call
    \ ld hl,(tape_start)    ; start of data
    \ push hl               ; needed by entry point $0984,
    \                       ; because it's done at the main entry point $0970
  C3 c, 0984 , C1 c,  DD c, 21 c, next , jpnext, end-code
    \ jp $0984              ; alternative entry point to SA_ALL,
    \                       ; after the save message
    \                       ; note: `jp` is used, but it works as a `call`,
    \                       ; because the return address has been pushed
    \ return_from_ROM:
    \ pop bc                ; restore Forth IP
    \ ld ix,next            ; restore Forth IX
    \ _jp_next

  \ doc{
  \
  \ (>tape-file) ( -- )
  \
  \ Write a tape file using the data stored at `tape-header`.
  \
  \ ``(>tape-file)`` is a factor of `>tape-file`.
  \
  \ }doc

: >tape-file ( ca1 len1 ca2 len2 -- )
  set-tape-filename set-tape-memory (>tape-file) ; ?)

  \ doc{
  \
  \ >tape-file ( ca1 len1 ca2 len2 -- )
  \
  \ Write a memory region _ca1 len1_ into a tape file _ca2
  \ len2_.
  \
  \ See: `tape-file>`, `(>tape-file)`, `>file`.
  \
  \ }doc

( last-tape-header display>tape-file tape-file>display )

[unneeded] last-tape-header ?(

need tape-header

tape-header /tape-header + constant last-tape-header ( -- ca )

  \ doc{
  \
  \ last-tape-header ( -- ca )
  \
  \ Address of the second tape header, which is used by the ROM
  \ routines while loading. Its structure is the identical to
  \ `tape-header`.
  \
  \ It can be used by the application to know the details of
  \ the last tape file that was loaded.
  \
  \ See: `last-tape-filename`, `last-tape-filetype`,
  \ `last-tape-start`, `last-tape-length`.
  \
  \ }doc

last-tape-header constant last-tape-filetype ( -- ca )

  \ doc{
  \
  \ last-tape-filetype ( -- ca )
  \
  \ Address of the file type (one byte) in `last-tape-header`.
  \
  \ See: `tape-filetype`.
  \
  \ }doc

last-tape-header 1+ constant last-tape-filename ( -- ca )

  \ doc{
  \
  \ last-tape-filename ( -- ca )
  \
  \ Address of the filename in `last-tape-header`.
  \
  \ See: `/tape-filename`, `tape-filename`.
  \
  \ }doc

last-tape-header 11 + constant last-tape-length ( -- a )

  \ doc{
  \
  \ tape-length ( -- a )
  \
  \ Address of the file length in `last-tape-header`.
  \
  \ See: `tape-length`.
  \
  \ }doc

last-tape-header 13 + constant last-tape-start ( -- a ) ?)

  \ doc{
  \
  \ tape-start ( -- a )
  \
  \ Address of the file start in `last-tape-header`.
  \
  \ See: `tape-start`.
  \
  \ }doc

[unneeded] display>tape-file ?( need >tape-file

: display>tape-file ( ca len -- )
  16384 6912 2swap >tape-file ; ?)

  \ doc{
  \
  \ display>tape-file ( ca len -- )
  \
  \ Write the display memory into a tape file _ca
  \ len_.
  \
  \ See: `>tape-file`.
  \
  \ }doc

[unneeded] tape-file>display ?( need tape-file>

: tape-file>display ( ca len -- ) 16384 0 tape-file> ; ?)

  \ doc{
  \
  \ display>tape-file ( ca len -- )
  \
  \ Write the display memory into a tape file _ca
  \ len_.
  \
  \ See: `>tape-file`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-12-04: Started adapting the tape words from the Afera
  \ library.
  \
  \ 2015-12-23: Changes.
  \
  \ 2016-04-10: Fixed. First working version.
  \
  \ 2016-04-10: Improved: no "Start tape" message.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-02-08: Use `cconstant` for byte constants. Compact the
  \ code, saving one block. Make `read-tape-file` and
  \ `write-tape-file` independent` for `need`. Improve
  \ documentation. Move `.tape` to the tests module.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-06: Rename the four main words after their disk
  \ file equivalents and update the order of the parameters
  \ accordingly. Improve the documentation. Compact the code to
  \ save one block.
  \
  \ 2017-03-22: Improve documentation.  Convert `tape-header`
  \ fields to constants. Add `last-tape-header` and its fields.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-07-26: Add `display>tape-file` and
  \ `tape-file>display`.

  \ vim: filetype=soloforth
