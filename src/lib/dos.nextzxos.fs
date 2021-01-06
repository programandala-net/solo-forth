  \ dos.nextzxos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202101061835.
  \ See change log at the end of the file.

  \ ===========================================================
  \ Description

  \ NextZXOS support.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017, 2018, 2020, 2021.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( /base-filename /filename-ext )

  \ XXX TODO Adapt to NextZXOS.

unneeding /base-filename ?\ 8 cconstant /base-filename

  \ doc{
  \
  \ /base-filename ( -- n ) "slash-basefilename"
  \
  \ Return the maximum length of a NextZXOS base filename, i.e.,
  \ a filename without drive, user area and extension.
  \
  \ See also: `/filename-ext`, `/filename`, `>filename`.
  \
  \ }doc

unneeding /filename-ext ?\ 3 cconstant /filename-ext

  \ doc{
  \
  \ /filename-ext ( -- n ) "slash-filename-ext"
  \
  \ Return the maximum length of a NextZXOS filename extension
  \ excluding the dot.
  \
  \ See also: `/filename`, `/base-filename`.
  \
  \ }doc

( (rename-file rename-file )

unneeding (rename-file ?(

code (rename-file ( ca1 ca2 -- ior )
  D1 c, E1 c, DD c, 21 c, 0127 , dos-ix-preserve-ip_ call,
  \ pop de
  \ pop hl
  \ ld ix,dos_rename
  \ call dos.ix.preserve_ip
  pushdosior jp, end-code ?)
  \ jp push_dos_ior

  \ Credit:
  \
  \ Adapted from DZX-Forth.

  \ doc{
  \
  \ (rename-file ( ca1 ca2 -- ior ) "paren-rename-file"
  \
  \ Rename filename _ca1_ (a $FF-terminated string) to filename
  \ _ca2_ (a $FF-terminated string) and return the I/O result
  \ code _ior_.
  \
  \ ``(rename-file`` is a factor of `rename-file`.
  \
  \ }doc

unneeding rename-file ?( need (rename-file

: rename-file ( ca1 len1 ca2 len2 -- ior )
  >filename >r >filename r> (rename-file ; ?)

  \ Credit:
  \
  \ Adapted from DZX-Forth.

  \ doc{
  \
  \ rename-file ( ca1 len1 ca2 len2 -- ior )
  \
  \ Rename the file named by the character string _ca1 len1_ to
  \ the name in the character string _ca2 len2_ and return the
  \ I/O error code _ior_.
  \
  \ Origin: Forth-94 (FILE EXT), Forth-2012 (FILE EXT).
  \
  \ See also: `(rename-file`, `delete-file`.
  \
  \ }doc

( get-1346 set-1346 )

unneeding get-1346 ?(

code get-1346 ( -- n1 n2 n3 n4 )
  D9 c, DD c, 21 c, 013C , dos-ix_ call, 06 c, 00 c,
  \   exx                           ; save Forth IP
  \   ld ix,dos_get_1346
  \   call dos.ix
  \   ld b,0
  4A c, C5 c, 4B c, C5 c, 4C c, C5 c, 4D c, C5 c,
  \   ld c,d                         ; first sector buffer of cache
  \   push bc
  \   ld c,e                         ; number of cache sector buffers
  \   push bc
  \   ld c,h                         ; first sector buffer of RAM disk
  \   push bc
  \   ld c,l                         ; number of RAM disk sector buffers
  \   push bc
  D9 c, jpnext, end-code ?)
  \   exx                            ; restore Forth IP
  \   _jp_next

  \ doc{
  \
  \ get-1346 ( -- n1 n2 n3 n4 ) "get-1-3-4-6"
  \
  \ Return the NextZXOS current configuration of RAM banks 1, 3, 4
  \ and 6, which are organized as an array of 128 sector
  \ buffers, each of 512 bytes. The cache and the RAM disk
  \ occupy two separate (contiguous) areas of this array.

  \ [horizontal]
  \ _n1_ :: first sector buffer of cache
  \ _n2_ :: number of cache sector buffers
  \ _n3_ :: first sector buffer of RAM disk
  \ _n4_ :: number of RAM disk sector buffers

  \
  \ See also: `set-1346`, `default-1346`, `bank`.
  \
  \ }doc

  \ XXX TODO -- Finish the documentation.

unneeding set-1346 ?(

code set-1346 ( n1 n2 n3 n4 -- )
  D9 c, E1 c, C1 c, 61 c, D1 c, C1 c, 51 c, DD c, 21 c, 013F ,
  \ exx                  ; save Forth IP
  \ pop hl               ; L = number of RAM disk sector buffers
  \ pop bc
  \ ld h,c               ; H = first buffer of RAM disk
  \ pop de               ; E = number of cache sector buffers
  \ pop bc
  \ ld d,c               ; D = first buffer of cache
  \ ld ix,dos_set_1346
  dos-ix_ call, D9 c, jpnext, end-code ?)
  \ call dos.ix
  \ exx                  ; restore Forth IP
  \ _jp_next

  \ doc{
  \
  \ set-1346 ( n1 n2 n3 n4 -- ) "set-1-3-4-6"
  \
  \ Set the NextZXOS configuration of RAM banks 1, 3, 4 and 6,
  \ which are organized as an array of 128 sector buffers, each
  \ of 512 bytes. The cache and the RAM disk occupy two
  \ separate (contiguous) areas of this array.

  \ [horizontal]
  \ _n1_ :: first sector buffer of cache
  \ _n2_ :: number of cache sector buffers
  \ _n3_ :: first sector buffer of RAM disk
  \ _n4_ :: number of RAM disk sector buffers

  \
  \ See also: `get-1346`, `default-1346`, `bank`.
  \
  \ }doc

  \ XXX TODO -- Finish the documentation.

( (delete-file delete-file )

unneeding (delete-file ?(

code (delete-file ( ca -- ior )
  E1 c, DD c, 21 c, 0124 , dos-ix-preserve-ip_ call,
  \ pop hl
  \ ld ix,dos_delete
  \ call dos.ix.preserve_ip
  pushdosior jp, end-code ?)
  \ jp push_dos_ior

  \ Credit:
  \
  \ Adapted from DZX-Forth.

  \ doc{
  \
  \ (delete-file ( ca -- ior ) "paren-delete-file"
  \
  \ Delete the disk file named in the $FF-terminated string
  \ _ca_ and return the I/O result code _ior_.
  \
  \ ``(delete-file`` is a factor of `delete-file`.
  \
  \ }doc

unneeding delete-file ?( need (delete-file

: delete-file ( ca len -- ior ) >filename (delete-file ; ?)

  \ Credit:
  \
  \ Adapted from DZX-Forth.

  \ doc{
  \
  \ delete-file ( ca len -- ior )
  \
  \ Delete the disk file named in the string _ca len_ and
  \ return the I/O result code _ior_.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See also: `(delete-file`, `rename-file`.
  \
  \ }doc

( r/o w/o s/r bin headed 'ctrl-z' )

  \ Credit:
  \
  \ Adapted from DZX-Forth.

unneeding r/o ?\ %001 cconstant r/o

  \ doc{
  \
  \ r/o ( -- fam ) "r-o"
  \
  \ Return the "read only" file access method _fam_.
  \
  \ See also: `w/o`, `r/w`, `s/r`, `bin`,
  \ `create-file`, `open-file`.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ }doc

unneeding w/o ?\ %010 cconstant w/o

  \ doc{
  \
  \ w/o ( -- fam ) "w-o"
  \
  \ Return the "write only" file access method _fam_.
  \
  \ See also: `r/o`, `r/w`, `s/r`, `bin`,
  \ `create-file`, `open-file`.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ }doc

unneeding s/r ?\ %101 cconstant s/r

  \ doc{
  \
  \ s/r ( -- fam ) "s-r"
  \
  \ Return the "shared read" file access method _fam_.
  \
  \ See also: `r/o`, `w/o`, `r/w`, `bin`,
  \ `create-file`, `open-file`.
  \
  \ }doc

unneeding bin ?\ need alias ' noop alias bin immediate

  \ doc{
  \
  \ bin ( fam1 -- fam2 )
  \
  \ Modify file access method _fam1_ to additionally select  a
  \ "binary", i.e., not line oriented, file access method,
  \ giving file access method _fam2_.
  \
  \ See also: `r/o`, `w/o`, `r/w`, `s/r`,
  \ `create-file`, `open-file`.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ }doc

code headed ( fam1 -- fam2 )
  E1 c, CB c, C0 08 07 * + 05 + c, pushhl jp, end-code ?)
  \ pop hl
  \ set 7,l
  \ jp pushl

  \ doc{
  \
  \ headed ( fam1 -- fam2 )
  \
  \ Modify file access method _fam1_ to additionally select a
  \ "headed", i.e., with an additional NextZXOS header, file
  \ access method, giving file access method _fam2_.
  \
  \ ``headed`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : headed ( fam1 -- fam2 ) 128 and ;
  \ ----

  \ See also: `bin`, `r/o`, `w/o`, `r/w`, `s/r`,
  \ `create-file`, `open-file`.
  \
  \ }doc

unneeding 'ctrl-z' ?\ $1A cconstant 'ctrl-z'

  \ doc{
  \
  \ 'ctrl-z' ( -- c ) "tick-control-z-tick"
  \
  \ _c_ is the character used by NextZXOS for padding the files,
  \ which is $1A.
  \
  \ }doc

( create-file )

  \ Credit:
  \
  \ Adapted from DZX-Forth.

need assembler

code (create-file ( ca fam fid -- fid ior )
  d pop, h pop, l d ld, h pop, d push, b push, e b ld, d c ld,
  \   pop de  ; E = fid
  \   pop hl  ; L = fam
  \   ld d,l  ; D = fam
  \   pop hl  ; HL = ca
  \   push de ; save fid
  \   push bc ; save the Forth IP
  \   ld b,e  ; B = fid
  \   ld c,d  ; C = fam
  \   ; Calculate the create action
  c 7 bit, c 7 res, 1 a ld#, z? rif a inc, rthen
  \   bit 7,c ; headed? (maybe set by 'headed')
  \   res 7,c
  \   ld a,1  ; create action 1: new file with header; position after the header
  \   jr nz,paren_open_file_.actions ; jump if headed
  \   ; Create a file with no header
  \   inc a   ; create action 2: new file without a header, position at 0
  \ paren_open_file_.actions:
  \   ; A = open/create action
  a d ld, a e ld, do-dos-open_ jp, end-code
  \   ld d,a  ; D = create action
  \   ld e,a  ; E = open action
  \   jp do_dos_open

  \ XXX TODO -- Document. Add cross-reference to
  \ `do-dos-open_`.

  \ ****************************
  \
  \ XXX FIXME
  \
  \ When 'headed' is used, 'create-file'
  \ fails with DOS error 23 (file not
  \ found), no matter the fam, e.g.:
  \
  \   s" headed" w/o headed create-file
  \
  \ It works fine without 'headed', or when
  \ the file already exists.
  \
  \ But 'open-file' works fine with and
  \ without 'header', and its code is
  \ identical except the actions.
  \
  \ Tried with ZX Spectrum +3e too, no
  \ difference.

: create-file ( ca len fam -- fid ior )
  >r >filename r> file-id if (create-file exit then
                          drop #-288 ;

  \ doc{
  \
  \ create-file ( ca len fam -- fid ior )
  \
  \ Create the file named in the character string specified by
  \ _ca len_, and open it with file access method _fam_.  If a
  \ file with the same name already exists, recreate it as an
  \ empty file.
  \
  \ If the  file  was  successfully created  and  opened, _ior_
  \ is  zero, _fid_ is the file identifier and the file has
  \ been positioned to the start of the file.  Otherwise _ior_
  \ is the I/O result code and _fid_ is undefined.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See also: `open-file`,  `r/o`, `w/o`, `r/w`, `s/r`, `bin`.
  \
  \ }doc

( close-file close-blocks )

  \ Credit:
  \
  \ Adapted from DZX-Forth.

unneeding close-file ?(

code (close-file ( fid -- ior )
  E1 c, C5 c, 45 c, dd c, 21 c, 0109 , dos-ix_ call, C1 c,
  \ pop hl          ; L = file identifier
  \ push bc         ; save Forth IP
  \ ld b,l          ; B = file identifier
  \ ld ix,dos_close
  \ call dos.ix
  \ pop bc          ; restore Forth IP
  pushdosior jp, end-code
  \ jp push_dos_ior

  \ doc{
  \
  \ (close-file ( fid -- ior ) "paren-close-file"
  \
  \ Close the file identified by _fid_ and return the I/O
  \ result code _ior_.
  \
  \ ``(close-file`` is a factor of `close-file`.
  \ ``(close-file`` closes the file, but does not update
  \ `file-ids`.
  \
  \ }doc

: close-file ( fid -- ior )
  dup >r (close-file dup 0<> file-ids r> + c! ; ?)

  \ doc{
  \
  \ close-file ( fid -- ior )
  \
  \ Close the file identified by _fid_ and return the I/O
  \ result code _ior_.
  \
  \ See also: `open-file`, `create-file`, `(close-file`.
  \
  \ }doc

unneeding close-blocks ?( need close-file need flush

: close-blocks ( -- ) blocks? 0= #-295 ?throw
                      flush block-fid @ close-file throw
                      block-fid on ; ?)

  \ doc{
  \
  \ close-blocks ( -- )
  \
  \ If no blocks file is open, `throw` error #-295. Otherwise
  \ execute `flush` and close the current blocks file.
  \
  \ See also: `open-blocks`, `blocks?`, `block-fid`,
  \ `close-file`.
  \
  \ }doc

( file-position file-size eof? )

unneeding file-position ?(

code file-position ( fid -- ud ior )
  E1 c, C5 c, 45 c, DD c, 21 c, 0133 , dos-ix_ call,
  \   pop hl ; L = fid
  \   push bc ; save Forth IP
  \   ld b,l ; fid
  \   ld ix,dos_get_position
  \   call dos.ix
  C1 c, E5 c, D5 c, pushdosior jp, end-code ?)
  \   pop bc ; restore Forth IP
  \   push hl ; low part of _ud_
  \   push de ; high part of _ud_
  \   jp push_dos_ior

  \ doc{
  \
  \ file-position ( fid -- ud ior )
  \
  \ Return the the current file position _ud_ for the file
  \ identified by _fid_, and the I/O result code _ior_. If
  \ _ior_ is non-zero, _ud_ is undefined.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See also: `reposition-file`, `file-size`, `open-file`,
  \ `create-file`.
  \
  \ }doc

unneeding file-size ?(

code file-size ( fid -- d )
  E1 c, C5 c, 45 c, DD c, 21 c, 0139 , dos-ix-ehl_ jp,
  end-code ?)
  \ pop hl
  \ push bc ; save the Forth IP
  \ ld b,l ; fid
  \ ld ix, $0139 ; GET EOF
  \ jp dos_ehl

  \ doc{
  \
  \ file-size ( fid - ud ior )
  \
  \ _ud_ is  the  size, in  bytes,  of  the file  identified by
  \ _fid_.  _ior_  is  the I/O result code.  This operation
  \ does not affect the  value returned by `file-position`.  If
  \ _ior_ is non-zero, _ud_ is undefined.
  \
  \ WARNING: ``file-size`` returns unpredictable results on the
  \ ZX Spectrum +2A/+2B/+3, because of a bug in the +3DOS ROM:
  \ _ud_ may be correct, or rounded to 128-byte blocks, or
  \ correspond to the previously checked file.  The bug was
  \ fixed in the improved ROM of the ZX Spectrum +3e.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See also: `file-position`, `open-file`.
  \
  \ }doc

  \ XXX TODO write 'flush-disk' and try it before every
  \ 'file-size', in order to make this work on the ZX Spectrum
  \ +3e.

unneeding eof? ?( need file-size need file-position need d=

: eof? ( fid -- f )
  dup file-size throw rot file-position d= ; ?)

  \ Credit:
  \ Copied from DZX-Forth.

  \ doc{
  \
  \ eof? ( fid -- f )
  \
  \ Is the file position of file referenced to by _fid_ at the
  \ end of the file, i.e. does its file position equals its
  \ file size?
  \
  \ See also: `file-size`, `file-position`, `create-file`,
  \ `open-file`.
  \
  \ }doc

( (cat )

need /base-filename need /filename-ext

13 cconstant /cat-entry

  \ doc{
  \
  \ /cat-entry ( -- n ) "slash-cat-entry"
  \
  \ A `cconstant`. Return size _n_, in bytes, of every entry of
  \ the `cat-buffer` used by `(cat` and prepared by `>cat`.
  \
  \ See also: `cat-entries`.
  \
  \ }doc

create cat-entries 4 c,

  \ doc{
  \
  \ cat-entries ( -- ca )
  \
  \ A `cvariable`. _ca_ is the address of a character
  \ containing the number of entries of the `cat-buffer` used
  \ by `(cat`.  Its default value is 4. The length of each
  \ entry is `/cat-entry` and cannot be changed.
  \
  \ WARNING: Every time `cat` or `acat` are executed, a new
  \ `cat-buffer` is allocated in the `stringer` by `>cat`.
  \ Depending on the value of ``cat-entries``, this can
  \ overwrite the current contents of the `stringer`, whose
  \ maximum length is `/stringer`.
  \
  \ }doc

: /cat-buffer ( -- len ) cat-entries c@ 1+ /cat-entry * ;

  \ doc{
  \
  \ /cat-buffer ( -- len ) "slash-cat-buffer"
  \
  \ Return the current length _len_ of the `cat-buffer`, using
  \ the values of `cat-entries` and `/cat-entry`.
  \
  \ }doc

variable cat-buffer

  \ doc{
  \
  \ cat-buffer ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ address of the buffer used by `(cat` , `.cat`, `.acat` and
  \ other words.
  \
  \ WARNING: Every time `cat` or `acat` are executed, the value
  \ of ``cat-buffer`` is updated with the address of a new
  \ space allocated in the `stringer` by `>cat`. Depending on
  \ the value of `cat-entries`, the current contents of the
  \ `stringer` (whose maximum length is `/stringer`) could be
  \ overwritten.
  \
  \ See also: `/cat-entry`.
  \
  \ }doc

: .filename-ext ( ca -- ) '.' emit /filename-ext type ;

  \ doc{
  \
  \ .filename-ext ( -- ca ) "dot-filename-ext"
  \
  \ Display the filename extension whose `/filename-ext`
  \ characters (left justified, space filled) are stored at
  \ _ca_. A dot separator is printed first, which is not
  \ included in the string at _ca_.
  \
  \ See also: `.filename`.
  \
  \ }doc

: .filename ( ca -- )
  /base-filename 2dup type + .filename-ext ; -->

  \ doc{
  \
  \ .filename ( ca -- ) "dot-filename"
  \
  \ Display the filename whose characters are stored at _ca_,
  \ in two parts: `/base-filename` characters (left justified,
  \ space filled) followed by `/filename-ext` characters (left
  \ justified, space filled).  The dot that separates the base
  \ filename from the filename extension is not included in the
  \ string at _ca_, but it's printed.
  \
  \ See also: `.filename-ext`.
  \
  \ }doc

( (cat )

: >cat-entry ( n -- ca ) /cat-entry * cat-buffer @ + ;

  \ doc{
  \
  \ >cat-entry ( n -- ca ) "to-cat-entry"
  \
  \ Convert `cat-buffer` entry _n_ to its address _ca_.
  \
  \ See also: `/cat-entry`.
  \
  \ }doc

variable full-cat  full-cat on

  \ doc{
  \
  \ full-cat ( -- a )
  \
  \ A `variable` _a_ is the address of a cell containing a
  \ flag. When the flag is `true`, `cat`, `wcat`, `acat` and
  \ `wacat` display also system files. When the flag is
  \ `false`, they don't. Other values are not supported. The
  \ default value is `true`.
  \
  \ See also: `>cat`.
  \
  \ }doc

: allocate-cat ( -- )
  /cat-buffer allocate-stringer cat-buffer ! ;

  \ doc{
  \
  \ allocate-cat ( -- )
  \
  \ Allocate space in the `stringer` and update `cat-buffer`
  \ with its address.
  \
  \ See also: `/cat-buffer`.
  \
  \ }doc

: >cat ( ca len -- ca1 ca2 x )
  >filename allocate-cat cat-buffer @ dup /cat-buffer erase
            cat-entries c@ 1+ $100 * full-cat @ abs or ;

  \ doc{
  \
  \ >cat ( ca len -- ca1 ca2 x ) "to-cat"
  \
  \ Convert filename _ca len_ (wildcards permitted) to the
  \ parameters needed by `(cat`:
  \

  \ [horizontal]
  \ _ca1_ :: address of $FF-terminated filename (wildcards permitted)
  \ _ca2_ :: address of buffer
  \ _x_ (low byte) :: filter: bit 0 set if system files are
  \ included (configurable by `full-cat`)
  \ _x_ (high byte) :: size of the buffer in entries, plus one (>=2)

  \ See also: `wcat`, `cat`, `cat-entries`.
  \
  \ }doc

: more-cat ( -- )
  cat-entries c@ >cat-entry cat-buffer @ /cat-entry move ;

  \ doc{
  \
  \ more-cat ( -- )
  \
  \ Copy the last catalogue entry of `cat-buffer` to the first
  \ position in the buffer, in order to get the next chunk of
  \ the catalogue.
  \
  \ ``more-cat`` is a factor of `wcat`.
  \
  \ See also: `(cat`.
  \
  \ }doc

: more-cat? ( n -- f ) cat-entries c@ 1+ = ;

  \ doc{
  \
  \ more-cat? ( n -- f ) "more-cat-question"
  \
  \ There may be more catalague entries to come after _n_ of
  \ them have been completed in `cat-buffer`?
  \
  \ ``more-cat?`` is a factor of `wcat` and `wacat`.
  \
  \ See also: `(cat`.
  \
  \ }doc

code (cat ( ca1 ca2 x -- n ior )
  D9 c, C1 c, D1 c, E1 c,
    \ exx, b pop, d pop, h pop,
  DD c, 21 c, 011E , dos-ix_ call, 48 c, 06 c, 00 c, C5 c,
    \ 011E ix ldp#, dos-ix_ call, b c ld, 0 b ld#, b push,
  D9 c, pushdosior jp, end-code
    \ exx, pushdosior jp, end-code

  \ doc{
  \
  \ (cat ( ca1 ca2 x -- n ior ) "paren-cat"
  \
  \ Fill a buffer _ca2_ with part of the directory (sorted),
  \ using filename stored at _ca1_. Input and output
  \ parameters:
  \

  \ [horizontal]
  \ _ca1_ :: address of $FF-terminated filename (wildcards permitted)
  \ _ca2_ :: address of buffer
  \ _x_ (low byte) :: bit 0 set if system files are included
  \ _x_ (high byte) :: size of the buffer in entries, plus one (>=2)
  \ _n_ :: number of completed entries in buffer (if non-zero, there may be more to come)
  \ _ior_ :: I/O result code (if non-zero, _n_ is undefined)

  \
  \ ``(cat`` is a factor of `wcat` and a direct interface to
  \ the DOS CATALOG NextZXOS routine.
  \
  \ Entry 0 of the buffer must be preloaded with the first
  \ filename required (or erased with zeroes). Entry 1 will
  \ contain the first matching filename greater than the
  \ preloaded entry (if any). If the buffer is too small for
  \ the catalogue, ``(cat`` can be called again with entry 0
  \ replaced by entry _n_ (task done by `more-cat`) to fetch
  \ the next part of the directory.
  \
  \ See also: `cat-buffer`, `cat-entries`, `/cat-entry`.
  \
  \ }doc

( wcat cat )

need (cat need 3dup need 3drop

: .cat-entry ( ca -- )
  dup .filename
      /base-filename /filename-ext + + @ 4 .r ."  KiB" ;

  \ doc{
  \
  \ .cat-entry ( ca -- ) "dot-cat-entry"
  \
  \ Display a catalogue entry stored at _ca_. Format of the
  \ entry:
  \
  \ - Bytes 0..7: Base filename (left justified, space filled)
  \ - Bytes 8..10: Filename extension (left justified, space filled)
  \ - Bytes 11..12: File size in kibibytes (binary)
  \
  \ The file size is the amount of disk space allocated to the
  \ file, not necessarily the same as the amount used by the
  \ file.
  \
  \ ``.cat-entry`` is a factor of `.cat`.
  \
  \ See also: `.filename`.
  \
  \ }doc

: .cat ( n -- ) 1 ?do i >cat-entry .cat-entry cr loop ;

  \ doc{
  \
  \ .cat ( n -- ) "dot-cat"
  \
  \ Display _n_ entries from `cat-buffer`.
  \
  \ ``.cat`` is a factor of `wcat`.
  \
  \ See also: `.cat-entry`, `.acat`.
  \
  \ }doc

: wcat ( ca len -- ) >cat begin  3dup (cat throw ?dup
                          while  dup .cat more-cat?
                          while  more-cat
                          repeat then 3drop ;

  \ doc{
  \
  \ wcat ( ca len -- ) "w-cat"
  \
  \ Show a wild-card disk catalogue using the wild-card
  \ filename _ca len_.
  \
  \ See also: `cat`, `wacat`, `.cat`, `(cat`, `more-cat`,
  \ `set-drive`.
  \
  \ }doc

: cat ( -- ) s" *.*" wcat ;

  \ doc{
  \
  \ cat ( -- )
  \
  \ Show a disk catalogue of the current drive.
  \
  \ See also: `wcat`, `acat`, `set-drive`.
  \
  \ }doc

( wacat acat )

need (cat need tab need 3dup need 3drop

: .acat ( n -- ) 1 ?do  i >cat-entry .filename tab loop ;

  \ doc{
  \
  \ .acat ( n -- ) "dot-a-cat"
  \
  \ Display _n_ entries from `cat-buffer`, in abbreviated
  \ format.
  \
  \ ``.acat`` is a factor of `wacat`.
  \
  \ See also: `.filename`, `.cat`.
  \
  \ }doc

: wacat ( ca len -- ) >cat begin  3dup (cat throw ?dup
                           while  dup .acat more-cat?
                           while  more-cat
                           repeat then 3drop ;

  \ doc{
  \
  \ wacat ( ca len -- ) "w-a-cat"
  \
  \ Show an abbreviated wild-card disk catalogue using the
  \ wild-card filename _ca len_.
  \
  \ See also: `acat`, `wcat`, `(cat`, `.acat`, `more-cat`,
  \ `set-drive`.
  \
  \ }doc

: acat ( -- ) s" *.*" wacat ;

  \ doc{
  \
  \ acat ( -- ) "a-cat"
  \
  \ Show an abbreviated disk catalogue of the current drive.
  \
  \ See also: `wacat`, `cat`, `set-drive`.
  \
  \ }doc

( write-byte read-byte write-line )

unneeding read-byte ?( need assembler

code read-byte ( fid -- c ior )
  h pop, b push, l b ld,
  0118 ix ldp#, dos-ix_ call,
  0 h ld#, c l ld, b pop, h push, pushdosior jp, end-code ?)

  \ XXX TODO -- Rewrite in Z80 opcodes.

  \ doc{
  \
  \ read-byte ( fid -- c ior )
  \
  \ Read byte _c_ from file _fid_, returning I/O result code
  \ _ior_.  If _ior_ is non-zero, _c_ is undetermined.
  \
  \ See also: `write-byte`, `reposition-file`, `file-position`.
  \
  \ }doc

unneeding write-byte ?( need assembler

code write-byte ( c fid -- ior )
  h pop, d pop, b push, l b ld, e c ld,
  011B ix ldp#, dos-ix_ call,
  b pop, pushdosior jp, end-code ?)

  \ XXX TODO -- Rewrite in Z80 opcodes.

  \ doc{
  \
  \ write-byte ( c fid -- ior )
  \
  \ Write byte _c_ to file _fid_, returning I/O result code
  \ _ior_.
  \
  \ See also: `read-byte`, `reposition-file`, `file-position`.
  \
  \ }doc

unneeding write-line ?( need newline

: write-line ( ca len fid -- ior )
      dup >r write-file ?dup if rdrop exit then
  newline r> write-file ; ?)

  \ Credit:
  \ Adapted from DZX-Forth.

  \ doc{
  \
  \ write-line ( ca len fid -- ior )
  \
  \ Write  _len_  characters  from  _ca_  followed  by  the
  \ line terminator returned by `newline` to the file
  \ identified by _fid_ starting at its current position.
  \ _ior_ is the I/O result code.
  \
  \ At the conclusion of the operation,  `file-position`
  \ returns the next file  position after the  last character
  \ written to  the  file, and  `file-size` returns  a  value
  \ greater than or equal to the value returned by
  \ `file-position`.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See also: `write-file`, `write-byte`, `read-line`,
  \ `create-file`, `open-file`.
  \
  \ }doc

( read-line )

need read-byte need eol? need char+

create read-line-fid 0 c,
  \ XXX TMP -- Use a local instead.

variable read-line-max-len
  \ XXX TMP -- Use a local instead.

variable read-line-len
  \ XXX TMP -- Use a local instead.

: read-line ( ca1 len1 fid -- len2 f ior )
  read-line-fid c! read-line-max-len ! read-line-len off
  begin
    read-line-fid c@ read-byte
    ?dup      if  drop read-line-len @ false rot exit then
     dup eol? if 2drop read-line-len @ true  0   exit then
    over c! char+ 1 read-line-len +!
    read-line-len @ read-line-max-len @ < while
  repeat drop read-line-len @ true 0 ;

  \ doc{
  \
  \ read-line ( ca1 len1 fid -- len2 f ior )
  \
  \ Read the next  line from  the file  specified by _fid_ into
  \ memory  at the  address _ca1_.  At  most  _len1_ characters
  \ are  read.  One line-terminating character, defined in
  \ `newline>`,  may be read into memory  at the end of the
  \ line, but  is not included in the count _len2_.  The line
  \ buffer provided by _ca1_ should be at least _len1_+1
  \ characters long.
  \
  \ If the operation succeeded, _f_  is true and _ior_ is zero.
  \ If a line terminator  was received before _len1_ characters
  \ were read, then _len2_ is the number of characters, not
  \ including the line terminator, actually read (0 <= _len2_
  \ <= _len1_). When _len1_ =  _len2_ the line terminator has
  \ yet to be reached.
  \
  \ If the operation is initiated when the value returned by
  \ `file-position` is equal to the value returned by
  \ `file-size` for the file identified by _fid_, _f_ is false,
  \ _ior_ is zero, and _len2_ is zero. If  _ior_ is non-zero,
  \ an exception occurred during  the operation and _ior_ is
  \ the I/O result code.
  \
  \ An ambiguous condition exists if the  operation is
  \ initiated when the value  returned by `file-position` is
  \ greater than the  value returned by `file-size` for the
  \ file identified by _fid_, or if the requested operation
  \ attempts to read portions of  the file not written.
  \
  \ At the conclusion of the operation,  `file-position`
  \ returns the next file  position after the last character
  \ read.
  \
  \ NOTE: This implementation of ``read-line`` is not fully
  \ standard, because 2-character line terminators are not
  \ supported.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See also: `read-file`, `read-byte`, `write-line`, `create-file`,
  \ `open-file`.
  \
  \ }doc

  \ XXX TODO -- Support 2-character line terminators.

( flush flush-drive drive-unused )

unneeding flush ?\ : flush ( -- ) save-buffers empty-buffers ;

  \ doc{
  \
  \ flush ( -- )
  \
  \ Perform the function of `save-buffers`, then unassign all
  \ block buffers with `empty-buffers`.
  \
  \ Origin: Forth-83 (Required Word Set), Forth-94 (BLOCK).
  \ Forth-2012 (BLOCK).
  \
  \ See also: `close-blocks`.
  \
  \ }doc

unneeding flush-drive ?(

code flush-drive ( c -- ior )
  E1 c, 7D c, DD c, 21 c, 0142 , dos-ix-preserve-ip_ call,
  \ pop hl
  \ ld a,l
  \ ld ix,dos_flush
  \ call dos.ix.preserve_ip
  pushdosior jp, end-code ?)
  \ jp push_dos_ior

  \ doc{
  \
  \ flush-drive ( c -- ior )
  \
  \ Write any pending headers, data, directory entries for
  \ drive _c_ ('A'..'P'), returning the I/O result code _ior_.
  \
  \ This word ensures that the disk is up to date. It can be
  \ called at any time, even when files are open.
  \
  \ See also: `set-drive`, `close-file`.
  \
  \ }doc

unneeding drive-unused ?(

code drive-unused ( c -- n ior )
  E1 c, 7D c, DD c, 21 c, 0121 , dos-ix-preserve-ip_ call,
  \ pop hl
  \ ld a,l
  \ ld ix,dos_free_space
  \ call dos.ix.preserve_ip
  E5 c, pushdosior jp, end-code ?)
  \ push hl
  \ jp push_dos_ior

  \ doc{
  \
  \ drive-unused ( c -- n ior )
  \
  \ Return unused kibibytes _n_ in drive _c_, and the I/O
  \ result code _ior_.
  \
  \ See also: `unused`, `farunused`.
  \
  \ }doc

( get-user set-user )

unneeding get-user ?(

code get-user ( -- n ior )
  3E c, FF c, DD c, 21 c, 0130 , dos-ix-preserve-ip_ call,
  \ ld a,$FF ; don't set the default user, but get it
  \ ld ix,dos_set_user
  \ call dos.ix.preserve_ip
  26 c, 00 c, 68 07 + c, E5 c, pushdosior jp, end-code ?)
  \ ld h,0
  \ ld l,a
  \ push hl ; return default user
  \ jp push_dos_ior

  \ doc{
  \
  \ get-user ( -- n ior )
  \
  \ Get the current user area _n_, i.e. the user area implied
  \ by all filenames that do not specify a user number.  _ior_
  \ is the I/O result code.
  \
  \ See also: `set-user`, `get-drive`.
  \
  \ }doc

unneeding set-user ?(

code set-user ( n -- ior )
  E1 c, 78 05 + c, DD c, 21 c, 0130 , dos-ix-preserve-ip_ call,
  \ pop hl
  \ ld a,l ; default user
  \ ld ix,dos_set_user
  \ call dos.ix.preserve_ip
  pushdosior jp, end-code ?)
  \ jp push_dos_ior

  \ doc{
  \
  \ set-user ( n -- ior )
  \
  \ Set the current user area _n_, i.e. the user area implied
  \ by all filenames that do not specify a user number.  _ior_
  \ is the I/O result code.
  \
  \ See also: `get-user`, `set-drive`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2021-01-04: Start. Copy from <dos.plus3dos.fs> (2017, 2018,
  \ 2020).
  \
  \ 2021-01-05: Delete `2-block-drives`. Move to the NextZXOS
  \ kernel all the words required to use the library blocks
  \ file: `read-file`, `bank-read-file`, `write-file`,
  \ `bank-write-file`, `reposition-file`, `>filename`, `r/w`,
  \ `open-file`, `(open-file`, `/filename`, `do-dos-open_`,
  \ `file-id`, `file-ids`, `#file-ids`.
  \
  \ 2021-01-06: Move `flush`, and `close-blocks` from the
  \ NextZXOS kernel. Finish and improve `close-blocks`.

  \ vim: filetype=soloforth
