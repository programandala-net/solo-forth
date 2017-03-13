  \ dos.gplusdos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703131346
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ G+DOS support.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( dos-in dos-out dos-in, dos-out, )

[unneeded] dos-in
?\ code dos-in ( -- ) DB c, #231 c, jpnext, end-code
  \ in a,(231)
  \ _jp_next

  \ doc{
  \
  \ dos-in ( -- )
  \
  \ Page in the Plus D memory.
  \
  \ See also: `dos-out`, `dos-in,`, `@dos`, `!dos`.
  \
  \ }doc

[unneeded] dos-out
?\ code dos-out ( -- ) D3 c, #231 c, jpnext, end-code
  \ out (231),a
  \ _jp_next

  \ doc{
  \
  \ dos-out ( -- )
  \
  \ Page out the Plus D memory.
  \
  \ See also: `dos-in`, `dos-out,`.
  \
  \ }doc

[unneeded] dos-in,
?\ need macro  macro dos-in, ( -- ) DB c, #231 c, endm
  \ in a,(231)

  \ doc{
  \
  \ dos-in, ( -- )
  \
  \ Compile the Z80 instruction ``in a,(231)``, which pages in
  \ the Plus D memory.
  \
  \ See also: `dos-out,`, `dos-in`.
  \
  \ }doc

[unneeded] dos-out,
?\ need macro  macro dos-out, ( -- ) D3 c, #231 c, endm
  \ out (231),a

  \ doc{
  \
  \ dos-out, ( -- )
  \
  \ Compile the Z80 instruction ``out (231),a``, which pages out
  \ the Plus D memory.
  \
  \ See also: `dos-in,`, `dos-out`.
  \
  \ }doc

( /ufia ufia1 ufia2 >ufiax >ufia1 >ufia2 )

[unneeded] /ufia ?\ 24 cconstant /ufia

  \ doc{
  \
  \ /ufia ( -- n )
  \
  \ _n_ is the length of a UFIA (User File Information Area), a
  \ 24-byte structure which describes a file.
  \
  \ See also: `ufia`, `ufia1`, `ufia2`.
  \
  \ }doc

[unneeded] ufia1 ?\ $3E01 constant ufia1

  \ doc{
  \
  \ ufia1 ( -- a )
  \
  \ _a_ is the address of G+DOS UFIA1 (in the Plus D memory).
  \ A UFIA (User File Information Area) is a 24-byte structure
  \ which describes a file.  See `ufia` for a detailed
  \ description.
  \
  \ See also: `/ufia`.
  \
  \ }doc

[unneeded] ufia2 ?\ $3E1A constant ufia2

  \ doc{
  \
  \ ufia2 ( -- a )
  \
  \ _a_ is the address of G+DOS UFIA2 (in the Plus D memory).
  \ A UFIA (User File Information Area) is a 24-byte structure
  \ which describes a file.  See `ufia` for a detailed
  \ description.
  \
  \ See also: `/ufia`.
  \
  \ }doc

[unneeded] >ufiax ?( need /ufia need dos-in need dos-out
: >ufiax ( a a -- ) /ufia dos-in cmove dos-out ; ?)

  \ doc{
  \
  \ >ufiax ( a1 a2 -- )
  \
  \ Move a UFIA (User File Information Area) from _a1_ to _a2_,
  \ with the Plus D Memory paged in.
  \
  \ ``>ufiax`` is a common factor of `>ufia1` and `>ufia2`.
  \
  \ See also: `ufia`, `/ufia`, `ufia1`, `ufia2`.
  \
  \ }doc

[unneeded] >ufia1
?\ need ufia1 need >ufiax : >ufia1 ( a -- ) ufia1 >ufiax ;

  \ XXX TODO --
  \ h pop, ufia1 d ldp#, /ufia b ldp#, dos-in, ldir,
  \ dos-out, jpnext, end-code

  \ doc{
  \
  \ >ufia1 ( a -- )
  \
  \ Move a UFIA (User File Information Area) from _a_ to
  \ `ufia1`.
  \
  \ See also: `>ufia2`, `>ufiax`, `ufia`, `/ufia`.
  \
  \ }doc

[unneeded] >ufia2
?\ need ufia2 need >ufiax : >ufia2 ( a -- ) ufia2 >ufiax ;

  \ doc{
  \
  \ >ufia2 ( a -- )
  \
  \ Move a UFIA (User File Information Area) from _a_ to
  \ `ufia2`.
  \
  \ See also: `>ufia1`, `>ufiax`, `ufia`, `/ufia`.
  \
  \ }doc

( ufia )

need /ufia

create ufia  /ufia allot  ufia /ufia erase

  \ doc{
  \
  \ ufia ( -- a )
  \
  \ Return constant address _a_ of a buffer used as UFIA (User
  \ File Information Area), a 24-byte structure which describes
  \ a file.
  \
  \ Solo Forth words use ``ufia`` for G+DOS calls.  G+DOS uses
  \ its own buffers `ufia1` and `ufia2` for internal
  \ operations.

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

  \ See also: `/ufia`, `ufia1`, `ufia2`, `>ufiax`.
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

  \ Note: The original UFIA field names are used, except
  \ `device`, whose original name is "lstr1":

ufia      constant dstr1   \ drive: 1, 2 or '*'
ufia 1+   constant fstr1   \ file directory number
ufia 2+   constant sstr1   \ stream number
ufia 3 +  constant device  \ device: 'D' or 'd'
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

need assembler need dos-in, need dos-out,

code get-drive ( -- n )
  b push,  \ save the Forth IP
  dos-in, 3ACE fta, dos-out,
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
  \ Configure `ufia` to use a code file in the current drive
  \ with filename _ca2 len2_, start address _ca1_ and length
  \ _len1_.
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
  \ Read a file from disk, using the data hold in `ufia` and
  \ the alternative destination zone _ca len_, following the
  \ following two rules:
  \
  \ 1. If _len_ is not zero, use it as the count of bytes that
  \ must be read from the file defined in `ufia` and use _ca_
  \ as destination address.
  \
  \ 2. If _len_ is zero, use the file length stored in the file
  \ header instead, and then check also _ca_: If _ca_ is not
  \ zero, use it as destination address, else use the file
  \ address stored in the file header instead.
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
  \ read _len2_ bytes and store them starting at address
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
  \ | `s" pic.scr" 16384 6912 file>` | Load the file using its original known values
  \ | `s" pic.scr" 16384 6144 file>` | Load only the bitmap to the original known address
  \ | `s" pic.scr"     0    0 file>` | Load the file using its original unknown values
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

: file-status ( ca len -- a ior) set-filename (file-status) ;

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
  \ `file-type`, `file-dir`, `find-file`, `file-dir#`,
  \ `file-dirdesc`.
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
: find-file ( ca len -- a | 0 ) file-status 0= and ; ?)

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
need hgfile need lbyte need dos-in, need dos-out,

code (file>screen) ( -- ior )

  b push,  \ save the Forth IP

  ufia ix ldp#, hgfile hook,  \ get the file
  nc? rif  \ no error?

    4000 d ldp#, #128 b ldp#,
      \ destination and count

    d h ldp,
    rbegin lbyte hook, d stap,
            \ a l ld, d push, b push, 1744 call, b pop, d pop,
              \ print HL
            d incp, b decp, b a ld, c or,
    z? runtil dos-out,
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

need assembler need dos-in, need dos-out, need patch

code g.100h ( u -- )
  h pop, b push,
  h push, patch hook, h pop, 1744 call, dos-out,
  b pop,  next ix ldp#,  jpnext, end-code
  \ Print _u_ using a routine of the Plus D ROM, paging it
  \ with a hook.

code g.100i ( u -- )
  h pop, b push,
  dos-in, 1744 call, dos-out,
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

need assembler need ufia1 need rest need dos-in,

code cd2 ( n -- ior )
  h pop,
  b push,  \ save the Forth IP
  dos-in, l a ld, ufia1 sta, rest hook,
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

need assembler need dos-in, need dos-out,
need ufia need ufia1 need set-filename

create (cd0-error ( -- a ) asm
  168E call, dos-out, b pop, next ix ldp#,
    \ $168E=BORD_REST (restore the border).
    \ Page out the Plus D memory.
    \ Restore the Forth registers.
  0000 h ldp#, 2066 h stp,
    \ Clear G+DOS D_ERR_SP.
  af push, ' dosior>ior jp, end-asm
    \ Return the ior.

code cd0 ( -- ior )

  b push, dos-in,
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
  dos-out, b pop, b pop, next ix ldp#, ' false jp, end-code

  \ XXX FIXME -- Since the first time #-1006 (no disk in drive)
  \ is returned because of a missing disk, it is the only error
  \ result, no matter the drive, no matter if disks are
  \ inserted or not.

( (cat wcat wacat cat acat )

[unneeded] (cat ?( need pcat need ufia need hd00 need >ufia1

code ((cat ( -- ior )
  C5 c, CF c, pcat c, C1 c, DD c, 21 c, next , F5 c,
  \ push bc
  \ rst $08
  \ defb pcat
  \ pop bc
  \ ld ix,next
  \ push af
  ' dosior>ior jp, end-code
  \ jp dos_ior_to_ior_

  \ doc{
  \
  \ ((cat ( -- ior )
  \
  \ Show a disk catalogue of the current drive, calling the
  \ corresponding G+DOS hook command, which uses the data in
  \ `ufia1`. Return error result _ior_.
  \
  \ ``((cat`` is a low-level factor of `(cat`.
  \
  \ }doc

: (cat ( b -- ) hd00 c! ufia >ufia1 ((cat throw ; ?)

  \ doc{
  \
  \ (cat ( b -- )
  \
  \ Show a disk catalogue of the current drive, using the data
  \ in `ufia`, being _b_ the type of catalogue:

  \ - $02 = abbreviated
  \ - $04 = detailed
  \ - $12 = abbreviated with wild-card
  \ - $14 = detailed with wild-card

  \ ``(cat`` is the common factor of all words that show disk
  \ catalogues: `cat`, `acat`, `wcat`, `wacat`.
  \
  \ See also: `((cat`, `set-drive`.
  \
  \ }doc

[unneeded] wcat ?( need set-filename need (cat
: wcat ( ca len -- ) set-filename $14 (cat ; ?)

  \ doc{
  \
  \ wcat ( ca len -- )
  \
  \ Show a wild-card disk catalogue of the current drive using
  \ the wild-card filename _ca len_.  See the Plus D manual for
  \ wild-card syntax.
  \
  \ See also: `cat`, `acat`, `wacat`, `(cat`, `set-drive`.
  \
  \ }doc

[unneeded] wacat ?( need set-filename need (cat
: wacat ( ca len -- ) set-filename $12 (cat ; ?)

  \ doc{
  \
  \ wacat ( ca len -- )
  \
  \ Show a wild-card abbreviated disk catalogue of the current
  \ drive using the wild-card filename _ca len_.  See the Plus
  \ D manual for wild-card syntax.
  \
  \ See also: `cat`, `acat`, `wcat`, `(cat`, `set-drive`.
  \
  \ }doc

[unneeded] cat ?\ need wcat : cat ( -- ) s" *" wcat ;

  \ doc{
  \
  \ cat ( -- )
  \
  \ Show a disk catalogue of the current drive.
  \
  \ See also: `acat`, `wcat`, `wacat`, `(cat`, `set-drive`.
  \
  \ }doc

[unneeded] acat ?( need wacat : acat ( -- ) s" *" wacat ;

  \ doc{
  \
  \ acat ( -- )
  \
  \ Show an abbreviated disk catalogue of the current drive.
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

( back-from-dos-error_ )

need assembler need dos-out,

create back-from-dos-error_ ( -- a ) asm
  168E call, dos-out, b pop, next ix ldp#,
    \ $168E=BORD_REST (restore the border).
    \ Page out the Plus D memory.
    \ Restore the Forth registers.
  0000 h ldp#, 2066 h stp,
    \ Clear G+DOS D_ERR_SP.
  af push, ' dosior>ior jp, end-asm
    \ Return the ior.

  \ XXX TODO -- Rewrite with Z80 opcodes.

  \ XXX TODO -- Use G+DOS routine HOOK_RET at $22C8 to do all
  \ at once.

  \ Reference: Plus D ROM routine D_ERROR ($182D), and command
  \ hook PATCH ($2226).

  \ doc{
  \
  \ back-from-dos-error_ ( -- a )
  \
  \ Return the address _a_ of Z80 code that can be set to be
  \ executed when a G+DOS routine throws an error, therefore
  \ preventing the DOS from returning to BASIC.  This is needed
  \ only when a Forth word calls G+DOS routines directly
  \ instead of using hook codes.
  \
  \ ``back-from-dos-error_`` works like an alternative end to
  \ words that use direct G+DOS calls:  The routine at
  \ ``back-from-dos-error_`` will leave the proper error result
  \ on the stack.
  \
  \ Usage example:

  \ ----
  \ b push, dos-in,
  \   \ Save the Forth IP.
  \   \ Page in the G+DOS memory.
  \
  \ back-from-dos-error_ h ldp#, h push, 2066 sp stp,
  \   \ Set G+DOS D_ERR_SP ($2066) so an error will go to
  \   \   `back-from-dos-error_` instead of returning to BASIC.
  \   \   This is needed because we are using direct calls to the
  \   \   G+DOS ROM instead of hook codes.
  \
  \ \ ... do whatever G+DOS direct call here ...
  \
  \ dos-out, h pop, b pop, next ix ldp#, ' false jp, end-code
  \   \ Page out the G+DOS memory.
  \   \ Consume the address of `back-from-dos-error_`
  \   \   that was pushed at the start.
  \   \ Restore the Forth IP.
  \   \ Restore the Forth IX.
  \   \ Return `false` _ior_ (no error).
  \ ----
  \
  \ }doc

( @dos c@dos  )

[unneeded] @dos ?(

need assembler need dos-in, need dos-out,

code @dos ( a -- x )
  h pop, dos-in, m e ld, h incp, m d ld,
         dos-out, d push, jpnext, end-code ?)
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

need assembler need dos-in, need dos-out,

code c@dos ( ca -- b )
  h pop, dos-in, m a ld,
         dos-out, pusha jp, end-code ?)

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

need assembler need dos-in, need dos-out,

code !dos ( x a -- )
  h pop, d pop, dos-in, e m ld, h incp, d m ld,
                dos-out, jpnext, end-code ?)
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

need assembler need dos-in, need dos-out,

code c!dos ( b ca -- )
  h pop, d pop, dos-in, e m ld,
                dos-out, jpnext, end-code ?)

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

need assembler need dos-vars need dos-in, need dos-out,

code @dosvar ( n -- x )
  h pop,
  dos-in, dos-vars d ldp#, d addp, m e ld, h incp, m d ld,
  dos-out, d push, jpnext, end-code ?)

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

need assembler need dos-vars need dos-in, need dos-out,

code c@dosvar ( n -- b )
  h pop, dos-in, dos-vars d ldp#, d addp, m a ld,
         dos-out, pusha jp, end-code ?)

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

need assembler need dos-vars need dos-in, need dos-out,

code !dosvar ( x n -- )
  h pop, dos-in, dos-vars d ldp#, d addp,
                   d pop, e m ld, h incp, d m ld,
         dos-out, jpnext, end-code ?)

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

need assembler need dos-vars need dos-in, need dos-out,

code c!dosvar ( b n -- )
  h pop, dos-in, dos-vars d ldp#, d addp, d pop, e m ld,
         dos-out, jpnext, end-code ?)

  \ doc{
  \
  \ c!dosvar ( b n -- )
  \
  \ Store _b_ into the G+DOS variable _n_.
  \
  \ See also: `c@dosvar`, `!dosvar`, `c!dos`.
  \
  \ }doc

( rename-file )

need dos-in, need dos-out, need back-from-dos-error_
need set-filename need ufia need >ufia1 need >ufia2

code (rename-file ( -- ior )

  C5 c, dos-in,
    \ push bc                ; save the Forth IP
    \ in a,(231)             ; page in the Plus D memory

    \ Now set G+DOS D_ERR_SP ($2066) so an error will go to
    \ `back-from-dos-error_` instead of returning to BASIC.
    \ This is needed because we are using direct calls to the
    \ G+DOS ROM instead of hook codes:

  21 c, back-from-dos-error_ , E5 c, ED c, 73 c, 2066 ,
    \ ld hl,back_from_dos_error_pfa
    \ push hl
    \ ld ($2066),sp

    \ The following code is a copy of G+DOS RENAME_RUN routine
    \ ($257C), except its final part, which originally returns
    \ to BASIC and therefore it has been modified:

  2626 call, 2559 call, CA c, 167C ,
    \ call swap_ufias        ; swap UFIA 1 & 2 in the DFCA
    \ call find_file_2559    ; does the 2nd filename exist?
    \ jp   z,rep_28          ; error #28 if it does exist
  2626 call, 2559 call, C2 c, 1678 ,
    \ call swap_ufias        ; swap UFIA 1 & 2 in the DFCA
    \ call find_file_2559    ; does the 1st filename exist?
    \ jp   nz,rep_26         ; error #26 if it doesn't exist
  23 c, D5 c, 11 c, 3E1F , EB c, 01 c, #10 , ED c, B0 c, D1 c,
    \ inc  hl                ; point to 1st filename
    \ push de                ; save track and sector of its catalogue entry
    \ ld   de,ufia2.nstr2    ; second filename
    \ ex   de,hl             ; HL = 2nd filename; DE = 1st filename
    \ ld   bc,10             ; filename length
    \ ldir                   ; rename
    \ pop  de                ; restore track and sector
  0584 call,
    \ call wsad              ; write the CATalogue sector

  dos-out, E1 c, C1 c, DD c, 21 c, next , ' false jp, end-code
    \ out (231),a            ; page out the Plus D memory
    \ pop hl                 ; consume the address of `back-from-dos-error_`
    \ pop bc                 ; restore the Forth IP
    \ ld ix,next             ; restore the Forth IX
    \ jp false_              ; exit returning a zero _ior_

  \ doc{
  \
  \ (rename-file ( -- ior )
  \
  \ Rename the file named by the filename stored in `ufia1` to
  \ the filename stored in `ufia2`.  and return error result
  \ _ior_.
  \
  \ ``(rename-file`` is a factor of ``rename-file`.
  \
  \ }doc

: rename-file ( ca1 len1 ca2 len2 -- ior )
  set-filename ufia >ufia2 set-filename ufia >ufia1
  (rename-file ;

  \ XXX TODO -- Invert the order of the used UFIA in order to
  \ save swapping them once in `(rename-file`.

  \ doc{
  \
  \ rename-file ( ca1 len1 ca2 len2 -- ior )
  \
  \ Rename the file named by the character string _ca1 len1_ to
  \ the name in the character string _ca2 len2_ and return
  \ error result _ior_.
  \
  \ Origin: Forth-94 (FILE EXT), Forth-2012 (FILE EXT).
  \
  \ }doc

  \ ===========================================================
  \ Change log

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
  \
  \ 2017-03-05: Remove old `need patch`. Fix documentation.
  \
  \ 2017-03-06: Prepare alternative implementation of `cat`.
  \
  \ 2017-03-07: Add `>ufia1`, `>ufia2`, `>ufiax`. Improve
  \ documentation. Rename "plusd-in/out" to "dos-in/out", after
  \ the notation used for +3DOS. Replace `cat` and related word
  \ with a new, simpler implementation which uses the `pcat`
  \ command code. Add `back-from-dos-error_` and `rename-file`.
  \
  \ 2017-03-10: Improve documentation.
  \
  \ 2017-03-11: Improve documentation.
  \
  \ 2017-03-13: Test `rename-file`. Rewrite it with Z80
  \ opcodes. Improve documentation.

  \ vim: filetype=soloforth
