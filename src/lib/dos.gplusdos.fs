  \ dos.gplusdos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550

  \ -----------------------------------------------------------
  \ Description

  \ G+DOS support.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2015..2016: Main development.
  \
  \ 2016-04-11: Start `plusd-in`, plusd-out`, `plusd-in,`,
  \ plusd-out,`.
  \
  \ 2016-04-26: Remove `char`.
  \
  \ 2016-08-04: Remove obsolete words `dosior>error` and
  \ `?dos-error`. Update and fix comments. Fix `(cat)`: the
  \ border was not restored.
  \
  \ 2016-08-10: Move `dosior>ior` to the DOS module of the
  \ kernel.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-04: Review the words to page in and page out the
  \ Plus D interface: Use the port to page in, not the hook;
  \ use Z80 opcodes, not `z80-asm`.
  \
  \ 2017-01-04: Compact the code, saving one block.
  \
  \ 2017-01-04: Convert `get-drive` and `set-drive` from
  \ `z80-asm` to `z80-asm,`; make them independent to `need`;
  \ improve their documentation.
  \
  \ 2017-01-05: Convert all assembly words from `z80-asm` to
  \ `z80-asm,`: `(delete-file)`, `(>file)`, `(<file)`,
  \ `(file>screen)`, `(<file-as-is)`, `(file?)`, `(cat)`,
  \ `@dos`, `c@dos`, `c!dos`, `!dos`, `@dosvar`, `c@dosvar`,
  \ `!dosvar`, `c!dosvar`.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-02-05: Fix needing of `set-drive`.
  \
  \ 2017-02-07: Use `cconstant` for byte constants
  \
  \ 2017-02-08: Update and complete the paging tests `g.100h`
  \ and `g.100i`. Make hook codes individually accessible to
  \ `need`. Improve `set-drive` to return an error result and
  \ move it to the kernel.
  \
  \ 2017-02-09: Improve all the Plus D memory fetch and store
  \ words: smaller and faster. Compact the code, saving 4
  \ blocks.
  \
  \ 2017-02-10: Rename `<file` to `file>`, and `<file-as-is` to
  \ `file-as-is>`.  The new names are consistent with the
  \ convention used in Forth.  Non-standard file words are
  \ being unified in G+DOS, TR-DOS and +3DOS. Add `ufia1` and
  \ `ufia2`.
  \
  \ 2017-02-11: Improve `file>` to support any useful
  \ combination of implicit or explicit parameters (address and
  \ length). Remove `file-as-is>`, which is not needed anymore.
  \ Improve documentation.
  \
  \ 2017-02-12: Add `file-status`. Replace old versions of
  \ `file?` with simpler `file-exists?`.  Add `file-start`,
  \ `file-length`, `file-type`, `file-dir`. Rename `>ufia` to
  \ `set-code-file`. Rename `filename!` to `set-filename`.
  \
  \ 2017-02-13: Make low-level factor words return an _ior_
  \ instead of a _dosior_. It's more useful and saves one byte
  \ per word pair. Rename `file-dir` to `file-dirdesc` and add
  \ `file-dir#`. Improve documentation of `cat` words.  Move
  \ the UFIA drive setting from `set-code-file` to lower
  \ `set-filename`; this fixes `file-status`.  Rewrite `(cat`
  \ to return an error result.
  \
  \ 2017-02-14: Add `find-file`.
  \
  \ 2017-02-15: Fix typo.
  \
  \ 2017-02-17: Fix markup in documentation.  Update cross
  \ references.  Change markup of inline code that is not a
  \ cross reference.

( plusd-in plusd-out plusd-in, plusd-out, ufia1 ufia2 )

[unneeded] plusd-in
?\ code plusd-in ( -- ) DB c, #231 c, jpnext, end-code
  \ in a,(231)
  \ _jp_next

  \ doc{
  \
  \ plusd-in ( -- )
  \
  \ Page in the Plus D memory.
  \
  \ See also: `plusd-out`, `plusd-in,`, `@dos`, `!dos`.
  \
  \ }doc

[unneeded] plusd-out
?\ code plusd-out ( -- ) D3 c, #231 c, jpnext, end-code
  \ out (231),a
  \ _jp_next

  \ doc{
  \
  \ plusd-out ( -- )
  \
  \ Page out the Plus D memory.
  \
  \ See also: `plusd-in`, `plusd-out,`.
  \
  \ }doc

[unneeded] plusd-in,
?\ need macro  macro plusd-in, ( -- ) DB c, #231 c, endm
  \ in a,(231)

  \ doc{
  \
  \ plusd-in, ( -- )
  \
  \ Compile the Z80 instruction ``in a,(231)``, which pages in
  \ the Plus D memory.
  \
  \ See also: `plusd-out,`, `plusd-in`.
  \
  \ }doc

[unneeded] plusd-out,
?\ need macro  macro plusd-out, ( -- ) D3 c, #231 c, endm
  \ out (231),a

  \ doc{
  \
  \ plusd-out, ( -- )
  \
  \ Compile the Z80 instruction ``out (231),a``, which pages out
  \ the Plus D memory.
  \
  \ See also: `plusd-in,`, `plusd-out`.
  \
  \ }doc

[unneeded] ufia1 ?\ $3E01 constant ufia1
  \ G+DOS UFIA1 (in the Plus D memory).

[unneeded] ufia2 ?\ $3E1A constant ufia2
  \ G+DOS UFIA1 (in the Plus D memory).

( ufia )

24 cconstant /ufia  create ufia  /ufia allot  ufia /ufia erase

  \ doc{
  \
  \ ufia ( -- a )
  \
  \ Return constant address _a_ of a buffer used as G+DOS User File
  \ Information Area.
  \
  \ |===
  \ | Offset | Bytes | Meaning
  \
  \ |      0 |     1 | Drive number (1, 2 or '*' ($2A) for current)
  \ |      1 |     1 | Directory entry number
  \ |      2 |     1 | Stream number
  \ |      3 |     1 | Device density type ('d'=DD, 'D'=SD)
  \ |      4 |     1 | Directory description
  \ |      5 |    10 | File name (padded with spaces)
  \ |     15 |     1 | File type
  \ |     16 |     2 | Length of file
  \ |     18 |     2 | Start address
  \ |     20 |     2 | Length of a BASIC program
  \ |     22 |     2 | Autostart line of a BASIC program
  \ |===

  \ See also: `/ufia`.
  \
  \ }doc

  \ XXX TODO --
  \ |===
  \ Code          Type                   CAT string   ROM-ID
  \
  \  0            ERASED (free entry)    (NA)         NA
  \  1            BASIC                  BAS          0
  \  2            NUMBER ARRAY           D.ARRAY      1
  \  3            STRING ARRAY           $.ARRAY      2
  \  4            CODE                   CDE          3
  \  5            48K SNAPSHOT           SNP 48k      NA
  \  6            MICRODRIVE             MD.FILE      NA
  \  7            SCREEN$                SCREEN$      NA
  \  8            SPECIAL                SPECIAL      NA
  \  9            128K SNAPSHOT          SNP 128k     NA
  \ 10            OPENTYPE               OPENTYPE     NA
  \ 11            EXECUTE                EXECUTE      NA
  \ |===

  \ doc{
  \
  \ /ufia ( -- b )
  \
  \ A constant that holds the length of a G+DOS UFIA (User File
  \ Information Area), which is 24 bytes.
  \
  \ See also: `ufia`.
  \
  \ }doc

  \ Note: The original UFIA field names are used, except
  \ `device`, whose original name is "lstr1":

ufia      constant dstr1   \ drive: 1, 2 or '*'
ufia 1+   constant fstr1   \ file directory number
ufia 2+   constant sstr1   \ stream number
ufia 3 +  constant device  \ device: "D" or "d"
  \ XXX TODO -- Remove `device`. The distinction is useless in Forth.
ufia 4 +  constant nstr1   \ directory description
ufia 5 +  constant nstr2   \ file name
ufia 15 + constant hd00    \ file type
ufia 16 + constant hd0b    \ file length
ufia 18 + constant hd0d    \ file start address
ufia 20 + constant hd0f    \ BASIC length without variables
ufia 22 + constant hd11    \ BASIC autorun line

'd' device c!  2 sstr1 c!  1 dstr1 c!
  \ Set default values of device, stream and drive

( --file-types-- )

0 cconstant basic-filetype
1 cconstant data-array-filetype
2 cconstant string-array-filetype
3 cconstant code-filetype

: --file-types-- ;

( hxfer ofsm hofile sbyte hsvbk cfsm pntp cops hgfile lbyte )

  \ Hook codes

[unneeded] hxfer  ?\ $33 cconstant hxfer
[unneeded] ofsm   ?\ $34 cconstant ofsm
[unneeded] hofile ?\ $35 cconstant hofile
[unneeded] sbyte  ?\ $36 cconstant sbyte
[unneeded] hsvbk  ?\ $37 cconstant hsvbk
[unneeded] cfsm   ?\ $38 cconstant cfsm
[unneeded] pntp   ?\ $39 cconstant pntp
[unneeded] cops   ?\ $3A cconstant cops
[unneeded] hgfile ?\ $3B cconstant hgfile
[unneeded] lbyte  ?\ $3C cconstant lbyte

( hldbk wsad sad rest heraz cops2 pcat hrsad hwsad otfoc )

  \ Hook codes (continued)

[unneeded] hldbk ?\ $3D cconstant hldbk
[unneeded] wsad  ?\ $3E cconstant wsad
[unneeded] sad   ?\ $3F cconstant sad
[unneeded] rest  ?\ $40 cconstant rest
[unneeded] heraz ?\ $41 cconstant heraz
[unneeded] cops2 ?\ $42 cconstant cops2
[unneeded] pcat  ?\ $43 cconstant pcat
[unneeded] hrsad ?\ $44 cconstant hrsad
[unneeded] hwsad ?\ $45 cconstant hwsad
[unneeded] otfoc ?\ $46 cconstant otfoc

( patch --directory-descriptions-- dos-vars )

  \ Hook codes (continued)

[unneeded] patch ?\ $47 cconstant patch

  \ Directory descriptions

[unneeded] --directory-descriptions ?(

01 cconstant basic-file-dir    02 cconstant data-array-dir
03 cconstant string-array-dir  04 cconstant code-file-dir
05 cconstant snapshot-48k-dir  06 cconstant microdrive-file-dir
07 cconstant screens$-file-dir 08 cconstant special-file-dir
09 cconstant snapshot-128k-dir 10 cconstant opentype-file-dir
11 cconstant execute-file-dir

: --directory-descriptions-- ; ?)

[unneeded] dos-vars ?\ 8192 constant dos-vars

  \ doc{
  \
  \ dos-vars ( -- a )
  \
  \ Address of the G+DOS variables in the Plus D memory.
  \
  \ See also: `@dosvar`, `c@dosvar`, `!dosvar`, `c!dosvar`.
  \
  \ }doc

( get-drive )

[unneeded] get-drive ?(

need assembler need plusd-in, need plusd-out,

code get-drive ( -- n )
  b push,  \ save the Forth IP
  plusd-in, 3ACE fta, plusd-out,
  b pop, next ix ldp#,  \ restore the Forth registers
  pusha jp, end-code ?)

  \ XXX TODO -- Rewrite in Z80 opcodes.

  \ XXX TODO -- Check this method:
  \ bit 0 of 3DD1

  \ doc{
  \
  \ get-drive ( -- n )
  \
  \ Get the current drive _n_ (1 or 2).
  \
  \ See also: `set-drive`.
  \
  \ }doc

( delete-file )

need assembler need ufia need heraz need set-filename

code (delete-file) ( -- ior )
  b push,  \ save the Forth IP
  ufia ix ldp#, heraz hook,  \ delete the file
  b pop, next ix ldp#, \ restore the Forth registers
  af push, ' dosior>ior jp, end-code

  \ doc{
  \
  \ (delete-file) ( -- ior )
  \
  \ Delete a disk file using the data hold in `ufia`.
  \ Return an error result _ior_.
  \
  \ This word is a factor of `delete-file`.
  \
  \ }doc

: delete-file ( ca len -- ior ) set-filename (delete-file) ;

  \ doc{
  \
  \ delete-file ( ca len -- ior )
  \
  \ Delete the disk file named in the string _ca len_ and
  \ return an error result _ior_.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See also: `(delete-file)`.
  \
  \ }doc

( -filename set-filename set-code-file )

need ufia need get-drive

10 cconstant /filename  \ max filename length

  \ doc{
  \
  \ /filename ( -- b )
  \
  \ A constant that returns the maximum length of a G+DOS
  \ filename.
  \
  \ See also: `set-filename`.
  \
  \ }doc

: -filename ( -- ) nstr2 /filename blank ;

  \ doc{
  \
  \ -filename ( -- )
  \
  \ Blank the filename in `ufia`, i.e. replace it with spaces.
  \
  \ See also: `/filename`, `set-filename`.
  \
  \ }doc

: set-filename ( ca len -- )
  -filename /filename min nstr2 swap cmove get-drive dstr1 c! ;

  \ doc{
  \
  \ set-filename ( ca len -- )
  \
  \ Configure `ufia` to use filename _ca len_ and the current
  \ drive.
  \
  \ See also: `-filename`, `/filename`.
  \
  \ }doc

: set-code-file ( ca1 len1 ca2 len2 -- )
  set-filename  hd0b !  hd0d !  3 hd00 c!  4 nstr1 c! ;

  \ doc{
  \
  \ set-code-file ( ca1 len1 ca2 len2 -- )
  \
  \ Configure `ufia` to use a code file in the current drive with
  \ filename _ca2 len2_, start address _ca1_ and length _len1_.
  \
  \ See also: `set-filename`.
  \
  \ }doc

( >file )

need assembler need ufia need set-code-file
need hofile need hsvbk need cfsm

code (>file) ( -- ior )

  b push,  \ save the Forth IP
  ufia ix ldp#,
  hofile hook, \ open the file and create its header
  nc? rif \ no error?
    hd0d d ftp, hd0b b ftp,  \ DE=start, BC=length
    hsvbk hook, \ save to file
    nc? rif  cfsm hook,  rthen  \ close the file if no error
  rthen  b pop, next ix ldp#,  \ restore the Forth registers
  af push, ' dosior>ior jp, end-code

  \ doc{
  \
  \ (>file) ( -- ior )
  \
  \ Save a file to disk using the data hold in `ufia` and
  \ return error result _ior_.
  \
  \ This word is a factor of `>file`.
  \
  \ }doc

: >file ( ca1 len1 ca2 len2 -- ior ) set-code-file (>file) ;

  \ doc{
  \
  \ >file ( ca1 len1 ca2 len2 -- ior )
  \
  \ Save memory region _ca1 len1_ to a file named by the string
  \ _ca2 len2_, and return error result _ior_.
  \
  \ See also: `file>`, `(>file)`.
  \
  \ }doc

( file> )

need assembler need ufia need set-code-file
need hgfile need lbyte need hldbk

code (file>) ( ca len -- ior )

  d pop, h pop, b push, h push, d push, ( ip ca len )

  ufia ix ldp#, hgfile hook,  \ get the file
  c? rif  d pop, d pop,  \ error, so drop the parameters
  relse
    hd00 d ldp#, 9 b ld#,  \ file header destination and count
    rbegin  lbyte hook, d stap, d incp,  rstep
    b pop, d pop, b tstp, z?  rif
      hd0b b ftp, d tstp, z? rif  hd0d d ftp,  rthen
    rthen  hldbk hook,
  rthen b pop, next ix ldp#, af push, ' dosior>ior jp, end-code

  \ doc{
  \
  \ (file>) ( ca len -- ior )
  \
  \ Read a file from disk, using the data hold in `ufia` and the
  \ alternative destination zone _ca len_, following the
  \ following two rules:
  \
  \ 1. If _len_ is not zero, use it as the count of bytes that
  \ must be read from the file defined in `ufia` and use _ca_ as
  \ destination address.
  \
  \ 2. If _len_ is zero, use the file length stored in the file
  \ header instead, and then check also _ca_: If _ca_ not zero,
  \ use it as destination address, else use the file address
  \ stored in the file header instead.
  \
  \ Return error result _ior_.
  \
  \ This word is a factor of `file>`.
  \
  \ See also: `file-address`, `file-length`.
  \
  \ }doc

: file> ( ca1 len1 ca2 len2 -- ior)
  2dup 2>r 2swap set-code-file 2r> (file>) ;

  \ doc{
  \
  \ file> ( ca1 len1 ca2 len2 -- ior )
  \
  \ Read the contents of a disk file, whose filename is defined
  \ by the string _ca1 len1_, to memory zone _ca2 len2_ (i.e.
  \ read _len2_ bytes and stored them starting at address
  \ _ca2_), or use the original address and length of the file
  \ instead, depending on the following rules:
  \
  \ 1. If _len2_ is not zero, use _ca2 len2_.
  \
  \ 2. If _len2_ is zero, use the original file length instead
  \ and then check also _ca2_: If _ca2_ is zero, use the
  \ original file address instead.
  \
  \ Return error result _ior_.
  \
  \ Example:
  \
  \ The screen memory has been saved to a disk file using the
  \ following command:

  \ ----
  \ 16384 6912 s" pic.scr" >file
  \ ----

  \ Therefore, its original address is 16384 and its original
  \ size is 6912 bytes.
  \
  \ Now there are four ways to load the file from disk:

  \ |===
  \ | Example                        | Result
  \
  \ | `s" pic.scr" 16384 6912 file>` | Load the file using its original values
  \ | `s" pic.scr"     0    0 file>` | Load the file using its original values
  \ | `s" pic.scr" 32768    0 file>` | Load the whole file to address 32768
  \ | `s" pic.scr" 32768  256 file>` | Load only 256 bytes to address 32768
  \ |===

  \ See also: `>file`, `(file>)`.
  \
  \ }doc

( file-status )

need assembler need ufia need set-filename
need hgfile need lbyte

code (file-status) ( -- a ior )
  ufia ix ldp#, ix push,
  b push,  \ save the Forth IP
  hgfile hook,  \ get the file
  nc? rif  \ no error?
    hd00 d ldp#, 9 b ld#,  \ file header destination and count
    rbegin  lbyte hook, d stap, d incp,  rstep
      \ Load the file header.
      a xor, \ set no error
  rthen  b pop, next ix ldp#, \ restore the Forth registers
  af push, ' dosior>ior jp, end-code

  \ XXX TODO --  Update also the file directory number
  \ (`fstr1`) and the directory description (`nstr1`).

  \ doc{
  \
  \ (file-status) ( -- a ior )
  \
  \ Return the status of the file whose name is hold in `ufia`.
  \ If the file exists, its file header is read into `ufia`,
  \ _a_ is the address returned by `ufia`, and _ior_ is zero.
  \ If the file does not exists, _a_ is useless and _ior_ is
  \ the corresponding error code.
  \
  \ This word is a low-level factor of `file-status`.
  \
  \ }doc

: file-status ( ca len -- a ior)
  set-filename (file-status) ;

  \ doc{
  \
  \ file-status ( ca len -- a ior )
  \
  \ Return the status of the file identified by the character
  \ string _ca len_. If the file exists, _ior_ is zero and _a_
  \ is the address returned by `ufia`.  Otherwise _ior_ is the
  \ corresponding I/O result code and _a_ is useless.
  \
  \ Origin: Forth-94 (FILE-EXT), Forth-2012 (FILE-EXT).
  \
  \ See also: `file-exists?`, `file-start`, `file-length`,
  \ `file-type`, `file-dir`.
  \
  \ }doc

( file-exists? file-start file-length file-type find-file )

[unneeded] file-exists?  ?( need file-status
: file-exists? ( ca len -- f ) file-status nip 0= ; ?)

  \ doc{
  \
  \ file-exists? ( ca len -- f )
  \
  \ If the file named in the character string _ca len_ is
  \ found, _f_ is _true_. Otherwise _f_ is _false_.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] file-start  ?( need file-status need ufia

: file-start ( ca1 len1 -- ca2 ior )
  file-status nip hd0d @ swap ; ?)

  \ doc{
  \
  \ file-start ( ca1 len1 -- ca2 ior )
  \
  \ Return the file start address of the file named in the
  \ character string _ca1 len1_. If the file was successfully
  \ found, _ior_ is zero and _ca2_ is the start address.
  \ Otherwise _ior_ is an exception code and _ca2_ is
  \ undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] file-length  ?( need file-status need ufia

: file-length ( ca1 len1 -- len2 ior )
  file-status nip hd0b @ swap ; ?)

  \ doc{
  \
  \ file-length ( ca1 len1 -- len2 ior )
  \
  \ Return the file length of the file named in the character
  \ string _ca1 len1_. If the file was successfully found,
  \ _ior_ is zero and _len2_ is the file length.  Otherwise
  \ _ior_ is an exception code and _len2_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] file-type  ?( need file-status need ufia

: file-type ( ca len -- n ior )
  file-status nip hd00 c@ swap ; ?)

  \ doc{
  \
  \ file-type ( ca len -- n ior )
  \
  \ Return the G+DOS file-type indentifier of the file named in
  \ the character string _ca len_. If the file was successfully
  \ found, _ior_ is zero and _n_ is the file-type identifier.
  \ Otherwise _ior_ is an exception code and _n_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] find-file  ?( need file-status
: find-file ( ca len -- f ) file-status 0= and ; ?)

  \ doc{
  \
  \ find-file ( ca len -- a | 0 )
  \
  \ If the file named in the character string _ca len_ is
  \ found, update the contents of `ufia` and return its address
  \ _a_. Otherwise return zero.
  \
  \ See also: `file-status`.
  \
  \ }doc

( file-dir# file-dirdesc )

[unneeded] file-dir#  ?( need file-status need ufia

: file-dir# ( ca len -- n ior )
  file-status nip fstr1 c@ swap ; ?)

  \ XXX FIXME --  _n_ is always zero. `file-status` does not
  \ update the whole UFIA?

  \ doc{
  \
  \ file-dir# ( ca len -- n ior )
  \
  \ Return the file directory number of the file named in the
  \ character string _ca len_. If the file was successfully
  \ found, _ior_ is zero and _n_ is the file directory number.
  \ Otherwise _ior_ is an exception code and _n_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] file-dirdesc  ?( need file-status need ufia

: file-dirdesc ( ca len -- n ior )
  file-status nip nstr1 c@ swap ; ?)

  \ XXX FIXME --  _n_ is always zero. `file-status` does not
  \ update the whole UFIA?

  \ doc{
  \
  \ file-dirdesc ( ca len -- n ior )
  \
  \ Return the G+DOS file directory identifier of the file
  \ named in the character string _ca len_. If the file was
  \ successfully found, _ior_ is zero and _n_ is the file
  \ directory identifier. Otherwise _ior_ is an exception code
  \ and _n_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

( file>screen )

  \ XXX UNDER DEVELOPMENT
  \ Experimental code to read lines from a file

need assembler need ufia need set-code-file
need hgfile need lbyte need plusd-in, need plusd-out,

code (file>screen) ( -- ior )

  b push,  \ save the Forth IP

  ufia ix ldp#, hgfile hook,  \ get the file
  nc? rif  \ no error?

    4000 d ldp#, #128 b ldp#,
      \ destination and count

    d h ldp,
    rbegin  lbyte hook, d stap,
            \ a l ld, d push, b push, 1744 call, b pop, d pop,
              \ print HL
            d incp, b decp, b a ld, c or,
    z? runtil  plusd-out,
    \ rbegin
    \   lbyte hook,  af push,  10 hook,  af pop,  13 cp#,
    \ z runtil

  rthen b pop, next ix ldp#, af push, ' dosior>ior jp, end-code
        \ restore the Forth registers and save the ior
  \ Print a file on the screen, line by line, using the data
  \ hold in UFIA.

: file>screen ( ca len -- ior )
  0 0 2swap set-code-file (file>screen) ;
  \ Copy a file _ca len_ to the screen, line by line,
  \ and return error result _ior_.

( g.100h g.100i )

  \ XXX TMP -- for debugging

need assembler need plusd-in, need plusd-out, need patch

code g.100h ( u -- )
  h pop, b push,
  h push, patch hook, h pop, 1744 call, plusd-out,
  b pop,  next ix ldp#,  jpnext, end-code
  \ Print _u_ using a routine of the Plus D ROM, paging it
  \ with a hook.

code g.100i ( u -- )
  h pop, b push,
  plusd-in, 1744 call, plusd-out,
  b pop,  next ix ldp#,  jpnext, end-code
  \ Print _u_ using a routine of the Plus D ROM, paging it
  \ with an `in` instruction.

( cd3 )

  \ `check-disk`, try 3

  \ XXX UNDER DEVELOPMENT -- 2017-02-13

need assembler need rest need get-drive

code (cd3 ( -- ior )
  b push, rest hook, b pop, next ix ldp#,
  af push, ' dosior>ior jp, end-code

: cd3 ( n -- ior )
  get-drive >r set-drive throw (cd3 r> set-drive throw ;

  \ XXX FIXME -- Since the first time #-1006 (no disk in drive)
  \ is returned because of a missing disk, it is the only error
  \ result, no matter the drive, no matter if disks are
  \ inserted or not.

( cd2 )

  \ `check-disk`, try 2

  \ XXX UNDER DEVELOPMENT -- 2017-02-13

need assembler need ufia1 need rest need plusd-in,

code cd2 ( n -- ior )
  h pop,
  b push,  \ save the Forth IP
  plusd-in, l a ld, ufia1 sta, rest hook,
  b pop, next ix ldp#, \ restore the Forth registers
  af push, ' dosior>ior jp, end-code

  \ XXX REMARK -- It seems the `rest` hook does not use the
  \ drive specified in UFIA passed in IX, like the RAMSOFT's
  \ _DISCiPLE/+D Technical Guide_ suggests (it's not clear, it
  \ reads "UFIA" but not IX) but the current drive.
  \
  \ This alternative uses G+DOS' UFIA1, just in case, but it
  \ does not work: _ior_ is always zero, no matter if there's a
  \ disk in the drive or not.

( cd1 )

  \ `check-disk`, try 1

  \ XXX UNDER DEVELOPMENT -- 2017-02-13

need assembler need ufia need rest

code cd1 ( n -- ior )
  h pop,
  b push,  \ save the Forth IP
  l a ld, dstr1 sta, ufia ix ldp#, rest hook,
  b pop, next ix ldp#, \ restore the Forth registers
  af push, ' dosior>ior jp, end-code

  \ XXX REMARK -- It seems the `rest` hook does not use the
  \ drive specified in UFIA passed in IX, like the RAMSOFT's
  \ _DISCiPLE/+D Technical Guide_ suggests (it's not clear, it
  \ reads "UFIA" but not IX) but the current drive.
  \
  \ XXX FIXME -- The method works: when the disk is removed,
  \ _ior_ is #-1006. The problem is #-1006 is returned also
  \ after inserting the disk, unless `set-drive` is used *on
  \ the other drive* first, and then on the other one.

( cd0 )

  \ `check-disk`, try 0

  \ XXX UNDER DEVELOPMENT -- 2017-02-13

  \ XXX TODO --

need assembler need plusd-in, need plusd-out, need patch
need ufia need ufia1 need set-filename need patch

create (cd0-error ( -- a ) asm
  168E call, plusd-out, b pop, next ix ldp#,
    \ $168E=BORD_REST (restore the border).
    \ Page out the Plus D memory.
    \ Restore the Forth registers.
  0000 h ldp#, 2066 h stp,
    \ Clear G+DOS D_ERR_SP.
  af push, ' dosior>ior jp, end-asm
    \ Return the ior.

code cd0 ( -- ior )

  b push,  plusd-in,
    \ Get the parameter.
    \ Save the Forth IP.
    \ Page in the Plus D memory.
  (cd0-error h ldp#, h push, 2066 sp stp,
    \ Set G+DOS D_ERR_SP ($2066) so an error will go to
    \   `(cd0-error` instead of returning to BASIC.
    \   This is needed because we are using direct calls to the
    \   G+DOS ROM instead of hook codes.
  \ 06C5 call, \ XXX REMARK -- This always returns #-1006
  06A4 call,  \ track_0
  plusd-out, b pop, b pop, next ix ldp#, ' false jp, end-code

  \ XXX FIXME -- Since the first time #-1006 (no disk in drive)
  \ is returned because of a missing disk, it is the only error
  \ result, no matter the drive, no matter if disks are
  \ inserted or not.

( (cat )

need assembler need plusd-in, need plusd-out, need patch
need ufia need ufia1 need set-filename need patch

create (cat-error ( -- a ) asm
  168E call, plusd-out, b pop, next ix ldp#,
    \ $168E=BORD_REST (restore the border).
    \ Page out the Plus D memory.
    \ Restore the Forth registers.
  0000 h ldp#, 2066 h stp,
    \ Clear G+DOS D_ERR_SP.
  af push, ' dosior>ior jp, end-asm
    \ Return the ior.

  \ XXX TODO -- Use G+DOS routine HOOK_RET at $22C8 to all at
  \ once.

  \ Reference: Plus D ROM routine D_ERROR ($182D), and command
  \ hook PATCH ($2226).

  \ doc{
  \
  \ (cat-error ( -- a )
  \
  \ Return the address _a_ of a subroutine that is executed
  \ when the Plus D ROM routines called by `(cat` throw an
  \ error (e.g., when there's no disk in the current drive),
  \ therefore preventing the system from returning to to BASIC.
  \
  \ This word works like an alternative ending of `(cat` to
  \ manage the error and return the proper error result.
  \
  \ }doc

code (cat ( n -- ior )

  d pop, b push,  plusd-in,
    \ Get the parameter.
    \ Save the Forth IP.
    \ Page in the Plus D memory.
  (cat-error h ldp#, h push, 2066 sp stp,
    \ Set G+DOS D_ERR_SP ($2066) so an error will go to
    \   `(cat-error` instead of returning to BASIC.
    \   This is needed because we are using direct calls to the
    \   G+DOS ROM instead of hook codes.

  exx,  ufia h ldp#, ufia1 d ldp#, /ufia b ldp#, ldir,
    \ Preserve the parameter into DE'
    \ Copy Forth UFIA to G+DOS UFIA1.

  exx, e a ld, 09A5 call, 168E call,
    \ 09A5 = SCAN_CAT  (input: cat or search type in the A register)
    \ XXX OLD: 24B5 = CAT_RUN (input: cat type in the A register)
    \ 168E = BORD_REST (restore the border)

  plusd-out, b pop, b pop, next ix ldp#, ' false jp, end-code
    \ Page out the Plus D memory.
    \ Consume the address of `(cat-error` that was pushed at the start.
    \ Restore the Forth registers and exit.

  \ doc{
  \
  \ (cat ( n -- )
  \
  \ Show a disk catalogue of the current drive, being _n_ the
  \ type of catalogue: 2=compact; 4=detailed.  This word is the
  \ low-level common factor of all words that show disk
  \ catalogues.
  \
  \ See also: `cat`, `acat`, `wcat`, `wacat`, `(cat-error`,
  \ `set-drive`.
  \
  \ }doc

( wcat cat wacat acat )

[unneeded] wcat ?( need (cat

: wcat ( ca len -- ) set-filename  4 (cat throw ; ?)

  \ doc{
  \
  \ wcat ( ca len -- )
  \
  \ Show a wild-card disk catologue of the current drive using
  \ the wild-card filename _ca len_.  See the Plus D manual for
  \ wild-card syntax.
  \
  \ See also: `cat`, `acat`, `wacat`, `(cat`, `set-drive`.
  \
  \ }doc

[unneeded] cat

?\ need wcat  : cat ( -- ) s" *" wcat ;

  \ doc{
  \
  \ cat ( -- )
  \
  \ Show a disk catalogue of the current drive.
  \
  \ See also: `acat`, `wcat`, `wacat`, `(cat`, `set-drive`.
  \
  \ }doc

[unneeded] wacat ?( need (cat

: wacat ( ca len -- ) set-filename  2 (cat throw ; ?)

  \ doc{
  \
  \ wacat ( ca len -- )
  \
  \ Show a wild-card abbreviated disk catologue of the current
  \ drive using the wild-card filename _ca len_.  See the Plus
  \ D manual for wild-card syntax.
  \
  \ See also: `cat`, `acat`, `wcat`, `(cat`, `set-drive`.
  \
  \ }doc

[unneeded] acat

?\ need wacat  : acat ( -- ) s" *" wacat ;

  \ doc{
  \
  \ acat ( -- )
  \
  \ Show an abbreviated disk catologue of the current drive.
  \
  \ See also: `cat`, `wcat`, `wacat`, `(cat`, `set-drive`.
  \
  \ }doc

  \ XXX TODO -- The disk catalogues can be printed out on a
  \ printer by storing the number 3 into SSTR1 (a field of UFIA
  \ that holds the stream number to use) before doing `CAT`.
  \ The default value is 2 (screen) and should be restored.
  \ Example:
  \
  \   3 sstr1 c! s" forth?.*" wcat 2 sstr1 c!

( @dos c@dos  )

[unneeded] @dos ?(

need assembler need plusd-in, need plusd-out,

code @dos ( a -- x )
  h pop, plusd-in, m e ld, h incp, m d ld,
         plusd-out, d push, jpnext, end-code ?)
  \ doc{
  \
  \ @dos ( a -- x )
  \
  \ Fetch the cell _x_ stored at Plus D memory address _a_.
  \
  \ See also: `!dos`, `c@dos`.
  \
  \ }doc

[unneeded] c@dos ?(

need assembler need plusd-in, need plusd-out,

code c@dos ( ca -- b )
  h pop, plusd-in, m a ld,
         plusd-out, pusha jp, end-code ?)

  \ doc{
  \
  \ c@dos ( ca -- b )
  \
  \ Fetch byte _b_ stored at Plus D memory address _ca_.
  \
  \ See also: `c!dos`, `@dos`.
  \
  \ }doc

( !dos c!dos )

[unneeded] !dos ?(

need assembler need plusd-in, need plusd-out,

code !dos ( x a -- )
  h pop, d pop, plusd-in, e m ld, h incp, d m ld,
                plusd-out, jpnext, end-code ?)
  \ doc{
  \
  \ !dos ( x a -- )
  \
  \ Store _x_ at the Plus D memory address _a_.
  \
  \ See also: `@dos`, `c!dos`.
  \
  \ }doc

[unneeded] c!dos ?(

need assembler need plusd-in, need plusd-out,

code c!dos ( b ca -- )
  h pop, d pop, plusd-in, e m ld,
                plusd-out, jpnext, end-code ?)

  \ doc{
  \
  \ c!dos ( b ca -- )
  \
  \ Store _b_ at the Plus D memory address _ca_.
  \
  \ See also: `c@dos`, `!dos`.
  \
  \ }doc


( @dosvar c@dosvar )

[unneeded] @dosvar ?(

need assembler need dos-vars need plusd-in, need plusd-out,

code @dosvar ( n -- x )
  h pop,
  plusd-in, dos-vars d ldp#, d addp, m e ld, h incp, m d ld,
  plusd-out, d push, jpnext, end-code ?)

  \ doc{
  \
  \ name ( -- )
  \
  \ Fetch the contents _x_ of G+DOS variable _n_.
  \
  \ See also: `!dosvar`, `c@dosvar`, `!dos`.
  \
  \ }doc

[unneeded] c@dosvar ?(

need assembler need dos-vars need plusd-in, need plusd-out,

code c@dosvar ( n -- b )
  h pop, plusd-in, dos-vars d ldp#, d addp, m a ld,
         plusd-out, pusha jp, end-code ?)

  \ doc{
  \
  \ c@dosvar ( n -- b )
  \
  \ Fetch the contents _b_ of G+DOS variable _n_.
  \
  \ See also: `c!dosvar`, `c!dosvar`, `@dos`.
  \
  \ }doc

( !dosvar c!dosvar )

[unneeded] !dosvar ?(

need assembler need dos-vars need plusd-in, need plusd-out,

code !dosvar ( x n -- )
  h pop, plusd-in, dos-vars d ldp#, d addp,
                   d pop, e m ld, h incp, d m ld,
         plusd-out, jpnext, end-code ?)

  \ doc{
  \
  \ !dosvar ( x n -- )
  \
  \ Store _x_ into the G+DOS variable _n_.
  \
  \ See also: `@dosvar`, `c!dosvar`, `!dos`.
  \
  \ }doc

[unneeded] c!dosvar ?(

need assembler need dos-vars need plusd-in, need plusd-out,

code c!dosvar ( b n -- )
  h pop, plusd-in, dos-vars d ldp#, d addp, d pop, e m ld,
                   plusd-out, jpnext, end-code ?)

  \ doc{
  \
  \ c!dosvar ( b n -- )
  \
  \ Store _b_ into the G+DOS variable _n_.
  \
  \ See also: `c@dosvar`, `!dosvar`, `c!dos`.
  \
  \ }doc

  \ vim: filetype=soloforth