  \ dos.trdos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703041849

  \ -----------------------------------------------------------
  \ Description

  \ TR-DOS support.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2016-08-04: Start: `dos-drive`, `get-drive`, `set-drive`.
  \
  \ 2017-02-05: Start from scratch. Move `get-drive` from the
  \ TR-DOS kernel and rewrite it in Z80. Move `cat` from the
  \ TR-DOS kernel.
  \
  \ 2017-02-10: Add DOS command constants. Add draft words to
  \ manipulate files. Rename "filedescriptor" notation to
  \ "fda". Add draft of `file-status`.
  \
  \ 2017-02-11: Fix `cat`.
  \
  \ 2017-02-12: Fix and document `file-status`. Add
  \ `file-exists?`, `file-start`, `file-length`, `file-type`,
  \ `file-dir`.  `file-sectors`, `file-sector`, `file-track`.
  \
  \ 2017-02-13: Fix `file-status`. Fix `file-dir` and rename it
  \ to `file-dir#`, after the changes in the G+DOS
  \ implementation.
  \
  \ 2017-02-14: Add `find-file`.
  \
  \ 2017-02-17: Restore the original TR-DOS notation
  \ "descriptor".  Update cross references.
  \
  \ 2017-03-04: Update the names of the DOS calls, after the
  \ changes in the kernel.

( --dos-commands-- )

  \ TR-DOS command codes

$00 cconstant dos-init-interface
$01 cconstant dos-init-drive
$02 cconstant dos-seek-track
$03 cconstant dos-set-sector
$04 cconstant dos-define-buffer
$05 cconstant dos-read-sectors
$06 cconstant dos-write-sectors
$07 cconstant dos-cat
$08 cconstant dos-read-file-descriptor
$09 cconstant dos-write-file-descriptor
$0A cconstant dos-find-file
$0B cconstant dos-create-file
$0C cconstant dos-save-basic-program
$0E cconstant dos-read-file
$12 cconstant dos-delete-file  -->

( --dos-commands-- )

  \ TR-DOS command codes (continued)

$13 cconstant dos-copy-from-hl-to-descriptor
$14 cconstant dos-copy-from-descriptor-to-hl
$15 cconstant dos-test-track
$16 cconstant dos-select-bottom-side
$17 cconstant dos-select-top-side
$18 cconstant dos-read-system-track

: --dos-commands-- ;

( fda /fda )

[unneeded] fda ?(

$5CDD     constant fda
fda       constant fda-filename
fda $08 + constant fda-filetype
fda $09 + constant fda-filestart
fda $0B + constant fda-filelength
fda $0D + constant fda-filesectors
fda $0E + constant fda-filesector
fda $0F + constant fda-filetrack ?)

  \ doc{
  \
  \ fda ( -- ca )
  \
  \ Return the address _ca_ of TR-DOS File Descriptor Area,
  \ which has the following structure:

  \ |===
  \ | Offset | Bytes | Contents
  \
  \ | +0x0   | 8     | File name
  \ | +0x8   | 1     | File type ('B, 'C', 'D', '#'...)
  \ | +0x9   | 2     | Address (or length of a BASIC program)
  \ | +0xB   | 2     | Length in bytes
  \ | +0xD   | 1     | Length in sectors
  \ | +0xE   | 1     | First sector of the file on its first track
  \ | +0xF   | 1     | First track of the file
  \ |===
  \
  \ See also: `/fda`.
  \
  \ }doc

[unneeded] /fda ?\ $10 cconstant /fda

  \ doc{
  \
  \ /fda ( -- b )
  \
  \ Return the length of TR-DOS `fda` (File Descriptor Area).
  \
  \ }doc

( /filename -filename set-filename )

[unneeded] /filename ?\ 9 cconstant /filename

  \ doc{
  \
  \ /filename ( -- n )
  \
  \ Return the maximum length of a TR-DOS filename, which is 9.
  \ In TR-DOS, the last character of the filename (character
  \ offset 8) is the filetype:
  \
  \ |===
  \ | Character | Filetype
  \
  \ | B         | BASIC program
  \ | C         | Code file
  \ | D         | BASIC data array file
  \ | #         | Serial/random access data file
  \ | other     | Defined by the programmer
  \ |===
  \
  \ See also: `set-filename`, `fda`.
  \
  \ }doc

[unneeded] -filename ?( need fda need /filename

: -filename ( -- )
  fda-filename /filename blank 'C' fda-filetype c! ; ?)

  \ doc{
  \
  \ -filename ( -- )
  \
  \ Erase the filename stored at TR-DOS `fda` (File Descriptor
  \ Area) with spaces, and set its type to 'C'.
  \
  \ See also: `set-filename`.
  \
  \ }doc

[unneeded] set-filename ?(

need -filename need /filename need fda

: set-filename ( ca len -- )
  -filename  /filename min fda smove ; ?)

  \ doc{
  \
  \ set-filename ( ca len -- )
  \
  \ Store filename _ca len_ into the TR-DOS `fda` (File Descriptor
  \ Area).  If _len_ is greater than 9 characters (the value
  \ returned by `/filename`), 9 is used instead.  If _ca len_
  \ does not include the file type at the end (at character
  \ offset +8), 'C' (code file) is used by default.
  \
  \ See also: `-filename`, `/filename`.
  \
  \ }doc

( get-drive cat )

[unneeded] get-drive

?\ code get-drive ( -- b ) 3A c, 5CF6 , pusha jp, end-code

  \ Note: $5CF6 is the TR-DOS variable "current temporary
  \ drive".

  \ doc{
  \
  \ get-drive ( -- b )
  \
  \ Return the number _b_ of the current drive (0..3).
  \
  \ This word is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : get-drive ( -- b )
  \   $5FC6 c@ ;
  \ ----

  \ See also: `set-drive`.
  \
  \ }doc

[unneeded] cat ?(

code cat ( -- ior )
  3E c, 07 c, 08 c, 3E c, 02 c, dos-alt-a-preserve-ip_ call,
  \ ld a,trdos_command.cat
  \ ex af,af'
  \ ld a,2 ; stream: screen
  \ call dos.alt_a.preserve_ip

  \ DD c, 21 c, next , \ ld ix,next ; restore Forth IX
  \ XXX REMARK -- No need to restore IX, the cat command does
  \ not use it.

  pushdosior jp, end-code ?)
  \ jp push_dos_ior

  \ doc{
  \
  \ cat ( -- ior )
  \
  \ Print a catalog of the current disk.
  \
  \ See also: `set-drive`.
  \
  \ }doc

( file> )

  \ XXX UNDER DEVELOPMENT

need assembler need --dos-commands-- need fda need set-filename

code (file>) ( ca len -- ior )

  d pop, h pop, b push, h push, d push, ( ip ca len )

  dos-find-file a ld#, exaf, dos-alt-a_ call,
  \ A = directory entry of the file, or $FF if not found
  a inc, z? rif  d pop, d pop, 1 a ld#,  relse
                 \ error, so drop parameters and report "no files"
    a dec, \ restore file descriptor (0..127)
    dos-read-file-descriptor c ld, dos-c_ call,
    z? rif  a xor, 5CF9 sta, d pop, h pop,
                 \ set load flag and get the parameters
       d tstp, nz?  \ is _len_ not zero?
       rif   03 a ld#,  \ if so, use the parameters
       relse h tstp, nz? rif cpl, rthen
         \ if _len_ is zero, use _ca_ if it's not zero
       rthen dos-read-file-descriptor c ld, dos-c_ call,
    rthen
  rthen b pop, pushdosior jp, end-code

  \ Note: TR-DOS command `dos-read-file` works different ways
  \ depending on the A register:
  \
  \ A=$00 - take address and length from FDA
  \ A=$03 - take address from HL and length from DE
  \ A=$FF - take address from HL but length from FDA
  \
  \ FDA = File Descriptor Area

: file> ( ca1 len1 ca2 len2 -- ior ) set-filename (file>) ;

( file-status )

need assembler need --dos-commands--
need fda need set-filename

code (file-status) ( -- a ior )

  fda h ldp#, h push, b push,
    \ Push `fda` (the _a_ returned) and save Forth IP.
  dos-find-file a ld#, exaf, dos-alt-a_ call,
    \ A = directory entry (0..127), or $FF if file not found
  a inc, z? rif  1 a ld#,  \ error: "no files"
  relse  a dec, \ restore directory entry (0..127)
         dos-read-file-descriptor c ld#, dos-c_ call,
         a xor,
          \ XXX REMAR -- This TR-DOS command does not
          \ return its error result in C, but the directory
          \ entry it received in A. Therefore, the value of
          \ A returned by the DOS call `dos-c_` is a
          \ copy of it.  We set it to zero (to force "no error").
  rthen b pop, pushdosior jp, end-code

: file-status ( ca len -- a ior )
  set-filename (file-status) ;

  \ doc{
  \
  \ file-status ( ca len -- a ior )
  \
  \ Return the status of the file identified by the character
  \ string _ca len_. If the file exists, _ior_ is zero and _a_
  \ is the address returned by `fda`, the TR-DOS File
  \ Descriptor Area; otherwise _ior_ is the corresponding I/O
  \ result code and _a_ is useless.
  \
  \ Origin: Forth-94 (FILE-EXT), Forth-2012 (FILE-EXT).
  \
  \ }doc

( file-exists? file-start file-length file-type find-file )

[unneeded] file-exists? ?( need file-status

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

[unneeded] file-start  ?( need file-status need fda

: file-start ( ca1 len1 -- ca2 ior )
  file-status nip fda-filestart @ swap ; ?)

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

[unneeded] file-len  ?( need file-status need fda

: file-length ( ca1 len1 -- len2 ior )
  file-status nip fda-filelength @ swap ; ?)

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

[unneeded] file-type  ?( need file-status need fda

: file-type ( ca len -- n ior )
  file-status nip fda-filetype c@ swap ; ?)

  \ XXX REMARK -- 2017-02-13: This word is uselsin TR-DOS,
  \ because the filetype is part of the filename.

  \ doc{
  \
  \ file-type ( ca len -- c ior )
  \
  \ Return the TR-DOS file-type indentifier of the file named
  \ in the character string _ca len_. If the file was
  \ successfully found, _ior_ is zero and _n_ is the file-type
  \ identifier.  Otherwise _ior_ is an exception code and _n_
  \ is undefined.
  \
  \ Note this word is useless in TR-DOS, because the file type
  \ in part of the filename. This word is included for
  \ compatibility with other DOSs supported.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] find-file ?( need file-status
: find-file ( ca len -- f ) file-status 0= and ; ?)

  \ doc{
  \
  \ find-file ( ca len -- a | 0 )
  \
  \ If the file named in the character string _ca len_ is
  \ found, return address _a_ of the updated `fda` (File
  \ Descriptor Area). Otherwise return zero.
  \
  \ See also: `file-status`.
  \
  \ }doc

( file-dir# )

need assembler need --dos-commands-- need set-filename

code (file-dir#) ( -- n ior )

  dos-find-file a ld#, exaf, dos-preserve-ip_ call,
    \ A = directory entry (0..127), or $FF if file not found
  0 h ld#, a l ld, h push,
  a inc, z? rif    1 a ld#,  \ error: "no files"
            relse  a xor,  rthen
  pushdosior jp, end-code

  \ doc{
  \
  \ (file-dir#) ( -- n ior )
  \
  \ Return the file directory number of the file whose filename
  \ is stored at `fda` (File Descriptor Area). If the file was
  \ successfully found, _ior_ is zero and _n_ is the file
  \ directory number.  Otherwise _ior_ is an exception code and
  \ _n_ is undefined.
  \
  \ See also: `file-dir#`, `file-status`.
  \
  \ }doc

: file-dir# ( ca len -- n ior ) set-filename (file-dir#) ;

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

( file-sectors file-sector file-track )

[unneeded] file-sectors  ?( need file-status need fda

: file-sectors ( ca len -- n ior )
  file-status nip fda-filesectors c@ swap ; ?)

  \ doc{
  \
  \ file-sectors ( ca len -- n ior )
  \
  \ Return the sectors occupied by the file named in the
  \ character string _ca len_. If the file was successfully
  \ found, _ior_ is zero and _n_ is the length in sectors.
  \ Otherwise _ior_ is an exception code and _n_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] file-sector  ?( need file-status need fda

: file-sector ( ca len -- n ior )
  file-status nip fda-filesector c@ swap ; ?)

  \ doc{
  \
  \ file-sector ( ca len -- n ior )
  \
  \ Return the first sector of the first track of the file
  \ named in the character string _ca len_. If the file was
  \ successfully found, _ior_ is zero and _n_ is the sector.
  \ Otherwise _ior_ is an exception code and _n_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

[unneeded] file-track  ?( need file-status need fda

: file-track ( ca len -- n ior )
  file-status nip fda-filetrack c@ swap ; ?)

  \ doc{
  \
  \ file-track ( ca len -- n ior )
  \
  \ Return the first track of the file named in the character
  \ string _ca len_. If the file was successfully found, _ior_
  \ is zero and _n_ is the track.  Otherwise _ior_ is an
  \ exception code and _n_ is undefined.
  \
  \ See also: `file-status`.
  \
  \ }doc

  \ vim: filetype=soloforth
