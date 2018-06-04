  \ dos.trdos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201806041135
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ TR-DOS support.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

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

unneeding fda ?(

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
  \ fda ( -- ca ) "f-d-a"
  \
  \ Return the address _ca_ of TR-DOS File Descriptor Area,
  \ which has the following structure:

  \ |===
  \ | Offset | Bytes | Address returned by | Contents
  \
  \ | +0x0   | 8     | `fda-filename`      | File name
  \ | +0x8   | 1     | `fda-filetype`      | File type ('B, 'C', 'D', '#'...)
  \ | +0x9   | 2     | `fda-filestart`     | Address (or length of a BASIC program)
  \ | +0xB   | 2     | `fda-filelength`    | Length in bytes
  \ | +0xD   | 1     | `fda-filesectors`   | Length in sectors
  \ | +0xE   | 1     | `fda-filesector`    | First sector of the file on its first track
  \ | +0xF   | 1     | `fda-filetrack`     | First track of the file
  \ |===

  \ See: `/fda`, `read-file-descriptor`,
  \ `write-file-descriptor`.
  \
  \ }doc

  \ doc{
  \
  \ fda-filename ( -- ca ) "f-d-a-filename"
  \
  \ First field of `fda` (File Descriptor Area).  _ca_ is the
  \ address of an 8-byte area that holds the filename.
  \
  \ WARNING: The actual filename is a 9-character string formed
  \ by the filename stored at ``fda-filename`` and the
  \ character stored at `fda-filetype.`
  \
  \ See: `/filename`.
  \
  \ }doc

  \ doc{
  \
  \ fda-filetype ( -- ca ) "f-d-a-file-type"
  \
  \ Second field of `fda` (File Descriptor Area).  _ca_ is the
  \ address of a byte containing the filetype identifier
  \ character.
  \
  \ Filetypes recognized by TR-DOS are 'B' for a BASIC program,
  \ 'C' for a code file (the default on Solo Forth); 'D' for a
  \ BASIC data array; '#' for sequential or random access data
  \ files.  Only 'C' is in files created by Solo Forth.  The
  \ programmer can use any character as filetype identifier.
  \
  \ WARNING: TR-DOS uses the filetype character as the last
  \ character (the 9th character) of `fda-filename`, i.e. a
  \ filename always has 9 characters, and the last one is the
  \ filetype identifier.  That's why `fda-filename` and
  \ ``fda-filetype`` are contiguous in `fda`. This is important
  \ in some cases, e.g.  `rename-file` and `delete-file`. In
  \ Solo Forth, when a filename does not include the filetype,
  \ 'C' (code file) is used as default filetype.
  \
  \ }doc

  \ doc{
  \
  \ fda-filestart ( -- a ) "f-d-a-file-start"
  \
  \ Third field of `fda` (File Descriptor Area).  _a_ is the
  \ address of a cell containing the file start address.  If
  \ `fda-filetype` is 'B' (BASIC program), this cell contains
  \ the length of the BASIC program, including its variables.
  \
  \ See: `fda-filelength`.
  \
  \ }doc

  \ doc{
  \
  \ fda-filelength ( -- a ) "f-d-a-file-length"
  \
  \ Fourth field of `fda` (File Descriptor Area).  _a_ is the
  \ address of a cell containing the file length in bytes.
  \
  \ See: `fda-filestart`, `fda-filesectors`.
  \
  \ }doc

  \ doc{
  \
  \ fda-filesectors ( -- ca ) "f-d-a-file-sectors"
  \
  \ Fifth field of `fda` (File Descriptor Area).  _ca_ is the
  \ address of a byte containing the file length in sectors.
  \
  \ See: `fda-filetrack`, `fda-filesector`,
  \ `fda-filelength`.
  \
  \ }doc

  \ doc{
  \
  \ fda-filesector ( -- ca ) "f-d-a-file-sector"
  \
  \ Sixth field of `fda` (File Descriptor Area).  _ca_ is the
  \ address of a byte containing the first sector of the file.
  \
  \ See: `fda-filetrack`, `fda-filesectors`.
  \
  \ }doc

  \ doc{
  \
  \ fda-filetrack ( -- ca ) "f-d-a-file-track"
  \
  \ Seventh field of `fda` (File Descriptor Area).  _ca_ is the
  \ address of a byte containing the first track of the file.
  \
  \ See: `fda-filesector`, `fda-filesectors`.
  \
  \ }doc

unneeding /fda ?\ $10 cconstant /fda

  \ doc{
  \
  \ /fda ( -- b ) "slash-f-d-a"
  \
  \ Return the length of TR-DOS `fda` (File Descriptor Area).
  \
  \ }doc

( files/disk /filename -filename -fda-filename )

unneeding files/disk ?\ 128 cconstant files/disk

  \ doc{
  \
  \ files/disk ( -- n ) "files-slash-disk"
  \
  \ Return the maximum number _n_ of files on a disk, including
  \ the deleted files, which is 128.
  \
  \ }doc

unneeding /filename ?\ 9 cconstant /filename

  \ doc{
  \
  \ /filename ( -- len ) "slash-filename"
  \
  \ Return the maximum length _len_ of a TR-DOS filename, which
  \ is 9.  In TR-DOS, the last character of the filename
  \ (character offset 8) is the filetype:

  \ |===
  \ | Character | Filetype
  \
  \ | B         | BASIC program
  \ | C         | Code file
  \ | D         | BASIC data array file
  \ | #         | Serial/random access data file
  \ | other     | Defined by the programmer
  \ |===

  \ If the filetype is not specified in a filename, 'C' is
  \ used.
  \
  \ See: `set-filename`, `fda`.
  \
  \ }doc

unneeding -filename ?( need /filename

: -filename ( ca -- )
  /filename 2dup blank + 1- 'C' swap c! ; ?)

  \ doc{
  \
  \ -filename ( ca -- ) "minus-filename"
  \
  \ Erase the filename stored at _ca_ and set its type to 'C'.
  \
  \ See: `-fda-filename`, `set-filename`.
  \
  \ }doc

unneeding -fda-filename ?( need fda need -filename

: -fda-filename ( -- ) fda-filename -filename ; ?)

  \ doc{
  \
  \ -fda-filename ( -- ) "minus-f-d-a-filename"
  \
  \ Erase the filename stored at TR-DOS `fda` (File Descriptor
  \ Area) with spaces, and set its type to 'C'.
  \
  \ See: `-filename`, `set-filename`.
  \
  \ }doc

( set-filename get-filename filename>filetype )

unneeding set-filename ?(

need -fda-filename need /filename need fda

: set-filename ( ca len -- )
  -fda-filename /filename min fda-filename smove ; ?)

  \ doc{
  \
  \ set-filename ( ca len -- )
  \
  \ Store filename _ca len_ into the TR-DOS `fda` (File
  \ Descriptor Area).  If _len_ is greater than 9 characters
  \ (the value returned by `/filename`), 9 is used instead.  If
  \ _ca len_ does not include the file type at the end (at
  \ character offset +8), 'C' (code file) is used by default.
  \
  \ See: `-fda-filename`, `/filename`.
  \
  \ }doc

unneeding get-filename ?( need /filename need fda

: get-filename ( -- ca len ) fda-filename /filename ; ?)

  \ doc{
  \
  \ get-filename ( -- ca len )
  \
  \ Return the filename _ca len_ that is stored in `fda` (File
  \ Descriptor Area).
  \
  \ See: `set-filename`, `fda-filename`, `/filename`,
  \ `filename>filetype`.
  \
  \ }doc

unneeding filename>filetype

?\ : filename>filetype ( ca len -- c ) + 1- c@ ;

  \ doc{
  \
  \ filename>filetype ( ca len -- c ) "filename-to-file-type"
  \
  \ Return the filetype _c_ of filename _ca len_. Note _len_ is
  \ assumed to be `/filename`, i.e., _ca len_ is a complete
  \ filename.
  \
  \ See: `set-filename`, `get-filename`.
  \
  \ }doc

( get-drive (acat acat )

unneeding get-drive

?\ code get-drive ( -- b ) 3A c, 5CF6 , pusha jp, end-code

  \ Note: $5CF6 is the TR-DOS variable "current temporary
  \ drive".

  \ doc{
  \
  \ get-drive ( -- b )
  \
  \ Return the number _b_ of the current drive (0..3).
  \
  \ ``get-drive`` is written in Z80. Its equivalent definition
  \ in Forth is the following:

  \ ----
  \ : get-drive ( -- b ) $5FC6 c@ ;
  \ ----

  \ See: `set-drive`.
  \
  \ }doc

unneeding (acat ?(

code (acat ( -- ior )
  3E c, 07 c, 08 c, 3E c, 02 c, dos-alt-a-preserve-ip_ call,
  \ ld a,trdos_command.cat
  \ ex af,af'
  \ ld a,2 ; stream: screen
  \ call dos.alt_a.preserve_ip
  pushdosior jp, end-code ?)
  \ jp push_dos_ior

  \ doc{
  \
  \ (acat ( -- ior ) "paren-a-cat"
  \
  \ Display an abbreviated catalogue of the current disk and
  \ return the I/O result code _ior_.  ``(acat`` is a factor of
  \ `acat`.
  \
  \ See: `set-drive`.
  \
  \ }doc

unneeding acat ?\ need (acat : acat ( -- ) (acat throw ;

  \ doc{
  \
  \ acat ( -- ior ) "a-cat"
  \
  \ Display an abbreviated catalogue of the current disk.
  \
  \ See: `set-drive`, `(acat`.
  \
  \ }doc

( 2-block-drives 3-block-drives 4-block-drives )

unneeding 2-block-drives ?( need set-block-drives

: 2-block-drives ( -- ) 1 0 2 set-block-drives ;

2-block-drives ?)

  \ doc{
  \
  \ 2-block-drives ( -- )
  \
  \ Set the first two drives as block drives, in normal order.
  \
  \ Note: For convenience, when this word is loaded, it's also
  \ executed.
  \
  \ See: `3-block-drives`, `4-block-drives`,
  \ `set-block-drives`.
  \
  \ }doc

unneeding 3-block-drives ?( need set-block-drives

: 3-block-drives ( -- ) 2 1 0 3 set-block-drives ;

3-block-drives ?)

  \ doc{
  \
  \ 3-block-drives ( -- )
  \
  \ Set the first three drives as block drives, in normal
  \ order.
  \
  \ Note: For convenience, when this word is loaded, it's also
  \ executed.
  \
  \ See: `2-block-drives`, `4-block-drives`,
  \ `set-block-drives`.
  \
  \ }doc

unneeding 4-block-drives ?( need set-block-drives

: 4-block-drives ( -- ) 4 3 1 0 4 set-block-drives ;

4-block-drives ?)

  \ doc{
  \
  \ 4-block-drives ( -- )
  \
  \ Set all 4 drives as block drives, in normal order.
  \
  \ Note: For convenience, when this word is loaded, it's also
  \ executed.
  \
  \ See: `2-block-drives`, `3-block-drives`,
  \ `set-block-drives`.
  \
  \ }doc

( file> )

need assembler need --dos-commands-- need fda need set-filename

code (file> ( ca len -- ior )

  d pop, h pop, b push, h push, d push, ( ip ca len )

  dos-find-file c ld#, dos-c_ call,
  \ C = directory entry of the file, or $FF if not found
  c a ld, c inc, z? rif d pop, h pop, 1 a ld#, relse
                \ error, so drop parameters
                \ and set dosior #1 ("no files")
    dos-read-file-descriptor c ld#, dos-c_ call,
    a xor, 5CF9 sta, d pop, h pop,
      \ set load flag (0) and get the parameters
    d tstp, nz? rif 03 a ld#,
                    \ DE (_len_) is not zero,
                    \ so use the parameters: make A=$03
                relse h tstp, nz? rif FF a ld#, rthen rthen
                    \ DE (_len_) is zero, so:
                    \   - if HL (_ca_) is non-zero,
                    \     use paremeter address,
                    \     but length from `fda`: make A=$FF
                    \   - if HL (_ca_) is zero,
                    \     use address and length from `fda`:
                    \     nothing is needed, because
                    \     A is already 0 after `tstp,`
    dos-read-file c ld#, dos-c_ call,
      \ read the file
  rthen b pop, pushdosior jp, end-code

  \ Note: TR-DOS command `dos-read-file` works different ways
  \ depending on the A register:
  \
  \ A=$00 - take address and length from FDA
  \ A=$03 - take address from HL and length from DE
  \ A=$FF - take address from HL but length from FDA
  \
  \ FDA = File Descriptor Area

  \ doc{
  \
  \ (file> ( ca len -- ior ) "paren-file-from"
  \
  \ Search the disk for the file whose filename is stored at
  \ `fda` and read its metadata into `fda`. Then read the file
  \ contents to memory zone _ca len_ or to the original memory
  \ zone of the file, depending on the following rules:
  \
  \ 1. If _len_ is not zero, read _len_ bytes from the file to
  \ address _ca_.
  \
  \ 2. If _len_ is zero, use the original length of the file
  \ insted, and then check _ca_: If _ca_ is not zero, use it as
  \ destination address, else use the original address of the
  \ file.
  \
  \ Return I/O result code _ior_.
  \
  \ ``(file>`` is a factor of `file>`.
  \
  \ See: `fda-filestart`, `fda-filelength`.
  \
  \ }doc

: file> ( ca1 len1 ca2 len2 -- ior )
  2swap set-filename (file> ;

  \ doc{
  \
  \ file> ( ca1 len1 ca2 len2 -- ior ) "file-from"
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
  \ Return I/O result code _ior_.
  \
  \ Example:
  \
  \ The screen memory (start address 16384 and size 6912 bytes)
  \ is saved to a disk file with `>file`:

  \ ----
  \ 16384 6912 s" pic.scr" >file
  \ ----

  \ Now there are several ways to load that file from disk:

  \ |===
  \ | Example                        | Result
  \
  \ | `s" pic.scr" 16384 6912 file>` | Load the file using its original known values
  \ | `s" pic.scr" 16384 6144 file>` | Load only the bitmap to the original known address
  \ | `s" pic.scr"     0    0 file>` | Load the file using its original unknown values
  \ | `s" pic.scr" 32768    0 file>` | Load the whole file to address 32768
  \ | `s" pic.scr" 32768  256 file>` | Load only 256 bytes to address 32768
  \ |===

  \
  \ }doc

( >file )

need assembler also assembler need l: previous
need --dos-commands-- need fda need set-filename

code (>file ( -- ior )

  b push, dos-read-system-track c ld#, dos-c_ call,
  0 rl# nz? ?jr,
  \   push bc ; save the Forth IP
  \
  \   ld c,trdos_command.read_system_track
  \   call dos.c
  \   jr nz,paren_to_file.exit ; jump if error
  \
  \   ; XXX FIXME -- 2017-02-10: When there's no disk in the drive,
  \   ; TR-DOS throws "Disc Error. Retry,Abort,Ignore?". "Retry" is
  \   ; useless; "Abort" exits to BASIC with "Tape loading error";
  \   ; "Ignore" crashes the system. The call to `read_system_track`
  \   ; does not return.
  \   ;
  \   ; There must be a way to avoid this and return an ior.
  \
  dos-find-file c ld#, dos-c_ call, c inc, nz? rif
  \   ld c,trdos_command.find_file
  \   call dos.c
  \   ; C = directory entry of the file (0..127), or $FF if not found
  \   inc c ; file not found?
  \   jr z,paren_to_file.file_not_found ; jump if no error
  \
  \   ; error: file found
  2 a ld#, 0 rl# jr,
  \   ld a,trdos_error.file_exists ; error code "file exists"
  \   jr paren_to_file.exit

  rthen fda-filelength d ftp, fda-filestart h ftp,
  dos-create-file c ld#, dos-c_ call,
  \ paren_to_file.file_not_found:
  \   ld de,(fda.filelength)
  \   ld hl,(fda.filestart)
  \   ld c,trdos_command.create_file
  \   call dos.c

  0 l: b pop, pushdosior jp, end-code
  \ paren_to_file.exit:
  \   pop bc  ; restore the Forth IP
  \   jp push_dos_ior

: >file ( ca1 len1 ca2 len2 -- ior )
  set-filename fda-filelength ! fda-filestart ! (>file ;

( fda-filestatus file-status )

unneeding fda-filestatus ?(

need assembler need --dos-commands-- need fda

code fda-filestatus ( -- a ior )

  fda h ldp#, h push, b push,
    \ Push `fda` (the _a_ returned) and save Forth IP.
  dos-find-file a ld#, exaf, dos-alt-a_ call,
    \ C = directory entry (0..127), or $FF if file not found
  c a ld, c inc, z? rif  1 a ld#,  \ dosior #1 ("no files")
  relse  dos-read-file-descriptor c ld#, dos-c_ call,
  rthen b pop, pushdosior jp, end-code ?)

  \ doc{
  \
  \ fda-filestatus ( -- a ior ) "f-d-a-file-status"
  \
  \ Return the status of the file whose filename is stored at
  \ `fda`. If the file exists, _ior_ is zero and _a_ is `fda`,
  \ the TR-DOS File Descriptor Area. Otherwise _ior_ is the I/O
  \ result code and _a_ is undefined.
  \
  \ See: `file-status`.
  \
  \ }doc

unneeding file-status ?( need fda-filestatus need set-filename

: file-status ( ca len -- a ior )
  set-filename fda-filestatus) ; ?)

  \ doc{
  \
  \ file-status ( ca len -- a ior )
  \
  \ Return the status of the file identified by the character
  \ string _ca len_. If the file exists, _ior_ is zero and _a_
  \ is `fda`, the TR-DOS File Descriptor Area.  Otherwise _ior_
  \ is the I/O result code and _a_ is undefined.
  \
  \ Origin: Forth-94 (FILE-EXT), Forth-2012 (FILE-EXT).
  \
  \ See: `file-exists?`, `file-start`, `file-length`,
  \ `file-type`, `find-file`, `file-dir#`, `file-sectors`,
  \ `file-sector`, `file-track`, `delete-file`, `rename-file`.
  \
  \ }doc

( file-exists? file-start file-length file-type find-file )

unneeding file-exists? ?( need file-status

: file-exists? ( ca len -- f ) file-status nip 0= ; ?)

  \ doc{
  \
  \ file-exists? ( ca len -- f ) "file-exists-question"
  \
  \ If the file named in the character string _ca len_ is
  \ found, _f_ is _true_. Otherwise _f_ is _false_.
  \
  \ See: `file-status`.
  \
  \ }doc

unneeding file-start  ?( need file-status need fda

: file-start ( ca1 len1 -- ca2 ior )
  file-status nip fda-filestart @ swap ; ?)

  \ doc{
  \
  \ file-start ( ca1 len1 -- ca2 ior )
  \
  \ Return the file start address of the file named in the
  \ character string _ca1 len1_. If the file was successfully
  \ found, _ior_ is zero and _ca2_ is the start address.
  \ Otherwise _ior_ is the I/O result code.  and _ca2_ is
  \ undefined.
  \
  \ See: `file-status`, `fda-filestart`.
  \
  \ }doc

unneeding file-len  ?( need file-status need fda

: file-length ( ca1 len1 -- len2 ior )
  file-status nip fda-filelength @ swap ; ?)

  \ doc{
  \
  \ file-length ( ca1 len1 -- len2 ior )
  \
  \ Return the file length of the file named in the character
  \ string _ca1 len1_. If the file was successfully found,
  \ _ior_ is zero and _len2_ is the file length.  Otherwise
  \ _len2_ is undefined and _ior_ is the I/O result code.
  \
  \ See: `file-status`, `fda-filelength`.
  \
  \ }doc

unneeding file-type  ?( need file-status need fda

: file-type ( ca len -- n ior )
  file-status nip fda-filetype c@ swap ; ?)

  \ doc{
  \
  \ file-type ( ca len -- c ior )
  \
  \ Return the TR-DOS file-type indentifier of the file named
  \ in the character string _ca len_. If the file was
  \ successfully found, _ior_ is zero and _c_ is its file-type
  \ identifier.  Otherwise _ior_ is the I/O result code and _c_
  \ is undefined.
  \
  \ Note: In TR-DOS the file type is the 9th character of the
  \ filename. When a filetype is not included in a filename,
  \ i.e. when the specified filename is shorter than 9
  \ characters, filetype 'C' (code file) is assumed by default.
  \ Therefore ``file-type`` is almost useless on TR-DOS.
  \
  \ See: `file-status`, `fda-filetype`.
  \
  \ }doc

unneeding find-file ?( need file-status
: find-file ( ca len -- f ) file-status 0= and ; ?)

  \ doc{
  \
  \ find-file ( ca len -- a | 0 )
  \
  \ If the file named in the character string _ca len_ is
  \ found, return address _a_ of the updated `fda` (File
  \ Descriptor Area). Otherwise return zero.
  \
  \ See: `file-status`.
  \
  \ }doc

( fda-filedir# file-dir# )

unneeding fda-filedir# ?(

need assembler need --dos-commands-- need set-filename

code fda-filedir# ( -- n ior )

  b push, dos-find-file c ld#, dos-c_ call,
    \ C = directory entry (0..127), or $FF if file not found
  c a ld, b pop, 0 h ld#, a l ld, h push,
  a inc, z? rif a inc, relse a xor, rthen
                \ dosior #1 ("no files") or no error
  pushdosior jp, end-code ?)

  \ doc{
  \
  \ fda-filedir# ( -- n ior ) "f-d-a-file-dir-slash"
  \
  \ Return the file directory number of the file whose filename
  \ is stored at `fda` (File Descriptor Area). If the file was
  \ successfully found, _ior_ is zero and _n_ is the file
  \ directory number.  Otherwise _ior_ is the I/O result code
  \ and _n_ is undefined.
  \
  \ See: `file-dir#`, `fda-filestatus`.
  \
  \ }doc

unneeding file-dir# ?( need fda-filedir# need set-filename

: file-dir# ( ca len -- n ior ) set-filename fda-filedir# ; ?)

  \ doc{
  \
  \ file-dir# ( ca len -- n ior ) "file-dir-slash"
  \
  \ Return the file directory number of the file named in the
  \ character string _ca len_. If the file was successfully
  \ found, _ior_ is zero and _n_ is the file directory number.
  \ Otherwise _ior_ is the I/O result code and _n_ is
  \ undefined.
  \
  \ See: `file-status`, `fda-filedir#`.
  \
  \ }doc

( file-sectors file-sector file-track )

unneeding file-sectors  ?( need file-status need fda

: file-sectors ( ca len -- n ior )
  file-status nip fda-filesectors c@ swap ; ?)

  \ doc{
  \
  \ file-sectors ( ca len -- n ior )
  \
  \ Return the sectors occupied by the file named in the
  \ character string _ca len_. If the file was successfully
  \ found, _ior_ is zero and _n_ is the length in sectors.
  \ Otherwise _ior_ is the I/O result code and _n_ is
  \ undefined.
  \
  \ See: `file-status`, `fda-filesectors`.
  \
  \ }doc

unneeding file-sector  ?( need file-status need fda

: file-sector ( ca len -- n ior )
  file-status nip fda-filesector c@ swap ; ?)

  \ doc{
  \
  \ file-sector ( ca len -- n ior )
  \
  \ Return the first sector of the first track of the file
  \ named in the character string _ca len_. If the file was
  \ successfully found, _ior_ is zero and _n_ is the sector.
  \ Otherwise _ior_ is the I/O result code and _n_ is
  \ undefined.
  \
  \ See: `file-status`, `fda-filesector`.
  \
  \ }doc

unneeding file-track  ?( need file-status need fda

: file-track ( ca len -- n ior )
  file-status nip fda-filetrack c@ swap ; ?)

  \ doc{
  \
  \ file-track ( ca len -- n ior )
  \
  \ Return the first track of the file named in the character
  \ string _ca len_. If the file was successfully found, _ior_
  \ is zero and _n_ is the track.  Otherwise _ior_ is the I/O
  \ result code and _n_ is undefined.
  \
  \ See: `file-status`, `fda-filetrack`.
  \
  \ }doc

( delete-file )

need assembler need --dos-commands--
need fda need set-filename

code (delete-file ( -- ior )
  b push,
  dos-find-file c ld#, dos-c_ call,
  \ C = directory entry of the file, or $FF if not found
  c a ld, c inc, z? rif  1 a ld#, \ dosior #1 ("no files")
                    relse dos-delete-file c ld#, dos-c_ call,
                    rthen b pop, pushdosior jp, end-code

  \ doc{
  \
  \ (delete-file ( -- ior ) "paren-delete-file"
  \
  \ Delete a disk file using the data hold in `dfa`, returning
  \ the I/O result code _ior_.
  \
  \ ``(delete-file`` is a factor of `delete-file`.
  \
  \ }doc

: delete-file ( ca len -- ior ) set-filename (delete-file ;

  \ doc{
  \
  \ delete-file ( ca len -- ior )
  \
  \ Delete the disk file named in the string _ca len_,
  \ returning the I/O result code _ior_.
  \
  \ Origin: Forth-94 (FILE), Forth-2012 (FILE).
  \
  \ See: `undelete-file`, `(delete-file`, `rename-file`,
  \ `file-status`.
  \
  \ }doc

( read-system-track )

need assembler need --dos-commands--

code read-system-track ( -- ior )
  dos-read-system-track a ld#, exaf,
  dos-alt-a-preserve-ip_ call, pushdosior jp, end-code

  \ doc{
  \
  \ read-system-track ( -- ior )
  \
  \ Read the system track of the current disk of TR-DOS.
  \
  \ }doc

( read-file-descriptor write-file-descriptor )

need assembler need --dos-commands--

code read-file-descriptor ( n -- ior )
  h pop, b push, l a ld,
  dos-read-file-descriptor c ld#, dos-c_ call,
  b pop, pushdosior jp, end-code
  \ XXX TODO -- Rename to `entry>fda`?

code write-file-descriptor ( n -- ior )
  h pop, b push, l a ld,
  dos-write-file-descriptor c ld#, dos-c_ call,
  b pop, pushdosior jp, end-code
  \ XXX TODO -- Rename to `fda>entry`?

( undelete-file )

need fda need files/disk need -filename need get-filename
need read-file-descriptor need write-file-descriptor

create tmp-filename /filename allot
  \ XXX TMP -- This buffer is used instead of the `stringer`
  \ because a TR-DISK can have 128 files. 128 string
  \ comparations would overwrite a string in the `stringer`,
  \ unless it's reallocated every time.
  \
  \ XXX TODO -- Write an alternative to keep the filename in
  \ the stringer and on the stack.

: undelete-fda ( -- ) $5D08 c@ fda-filename c! ;

  \ doc{
  \
  \ undelete-fda ( -- ) "undelete-f-d-a"
  \
  \ Restore the first character of `fda-filename` with the
  \ character hold in the TR-DOS variable $5D08, which holds
  \ the first filename character of the latest deleted file.
  \
  \ ``undelete-fda`` is a factor of `undelete-file`.
  \
  \ }doc

: undelete-file ( ca len -- ior )
  tmp-filename -filename tmp-filename smove 1 tmp-filename c!
  read-system-track ?dup if unloop exit then
  files/disk 0 ?do
    i read-file-descriptor ?dup if unloop exit then
    get-filename tmp-filename /filename str=
    if undelete-fda i write-file-descriptor unloop exit then
  loop #-1001 ;

  \ Note:
  \
  \   #-1001 = TR-DOS ior for "no files"

  \ XXX TODO -- Improve: `read-file-descriptor` reads the
  \ system track every time. Explore the sector buffer instead.

  \ doc{
  \
  \ undelete-file ( ca len -- ior )
  \
  \ Undelete the disk file named in the string _ca len_,
  \ returning the I/O result code _ior_.
  \
  \ TR-DOS deletes a file replacing its first character with
  \ byte 1.  ``undelete-file`` replaces the first character in
  \ _ca len_ with byte 1, then searches the disk for such
  \ filename and restores its first character using the first
  \ character removed from the latest deleted file, which
  \ TR-DOS keeps in its variable $5D08.
  \
  \ Therefore, the procedure has some issues:
  \
  \ 1. If _ca len_ is not the latest deleted file, the first
  \ character of its filename will not be the original one.
  \
  \ 2. If more than one file has been deleted, with only the
  \ first character of their filenames being different in all
  \ of them, ``undelete-file`` will find the oldest one.
  \
  \ 3. TR-DOS does not reuse the space occupied by a deleted
  \ file, until the disk is defragmented
  \
  \ See: `delete-file`.
  \
  \ }doc

  \ XXX REMARK -- The TR-DOS command `dos-find-file` can not
  \ locate deleted files, because it ignores filenames with a
  \ byte 1 as first character, which is the deletion mark.
  \ That's why an alternative was needed.

( .filename .fda-filename fda-basic? fda-deleted? fda-empty? )

unneeding .filename ?( need /filename

: .filename ( ca -- )
  /filename 1- 2dup type '<' emit + c@ emit '>' emit ; ?)
  \ XXX TODO -- Call the ROM routine instead.

  \ doc{
  \
  \ .filename ( ca -- ) "dot-filename"
  \
  \ Display the filename stored at _ca_, using the TR-DOS
  \ filename format.
  \
  \ See: `.fda-filename`, `/filename`.
  \
  \ }doc

unneeding .fda-filename ?( need fda need .filename

: .fda-filename ( -- ) fda-filename .filename ; ?)

  \ doc{
  \
  \ .fda-filename ( -- ) "dot-f-d-a-filename"
  \
  \ Display the contents of `fda-filename`, using the TR-DOS
  \ filename format.
  \
  \ See: `.filename`, `/filename`.
  \
  \ }doc

unneeding fda-basic?

?\ need fda : fda-basic? ( -- f ) fda-filetype c@ 'B' = ;

  \ doc{
  \
  \ fda-basic? ( -- f ) "f-d-a-basic-question"
  \
  \ _f_ is true if `fda` contains a BASIC program file.
  \
  \ See: `fda-empty?`, `fda-deleted?`.
  \
  \ }doc

unneeding fda-deleted?

?\ need fda : fda-deleted? ( -- ) fda-filename c@ 1 = ;

  \ doc{
  \
  \ fda-deleted? ( -- f ) "f-d-a-deleted-question"
  \
  \ _f_ is true if `fda` contains a deleted file.
  \
  \ See: `fda-empty?`, `fda-basic?`.
  \
  \ }doc

unneeding fda-empty?

?\ need fda : fda-empty? ( -- f ) fda c@ 0= ;

  \ doc{
  \
  \ fda-empty? ( -- f ) "f-d-a-empty-question"
  \
  \ _f_ is true if `fda` is empty, i.e. it's unused, it does
  \ not contain a file descriptor.
  \
  \ See: `fda-deleted?`, `fda-basic?`.
  \
  \ }doc

( cat )

need --dos-commands-- need files/disk need read-system-track
need read-file-descriptor need u.r need fda need .fda-filename
need fda-basic? need fda-empty? need fda-deleted?

: cat-fda ( n -- )
  3 .r space .fda-filename fda-filesectors c@ 3 u.r
  6 fda-basic? if spaces else fda-filestart @ swap u.r then
  fda-filelength @ 6 u.r cr ;

  \ XXX TODO -- Add the BASIC autorun line. The problem is the
  \ information is not in File Descriptor Area, but at the
  \ start of the file contents, on its first sector. See TR-DOS
  \ routine at $131B.

  \ doc{
  \
  \ cat-fda ( n -- ) "cat-f-d-a"
  \
  \ Display catalogue entry _n_ of the current drive.
  \ The entry is already stored in `fda`.
  \
  \ ``cat-fda`` is a factor of `?cat-fda`.
  \
  \ See: `.fda-filename`, `fda-basic?`.
  \
  \ }doc

: ?cat-fda ( n -- ) fda-deleted? if drop exit then cat-fda ;

  \ doc{
  \
  \ ?cat-fda ( n -- ) "question-cat-f-d-a"
  \
  \ If catalogue entry _n_ of the current drive is not a
  \ deleted file, display it.  The entry is already stored at
  \ `fda`.
  \
  \ ``?cat-fda`` is a factor of `cat`.
  \
  \ See: `fda-deleted?`, `cat-fda`.
  \
  \ }doc

: cat ( -- )
  read-system-track throw  cr
  files/disk 0 ?do i read-file-descriptor throw
                   fda-empty? if leave then i ?cat-fda
               loop ;

  \ XXX TODO -- Improve: `read-file-descriptor` reads the
  \ system track every time. Explore the sector buffer instead.

  \ doc{
  \
  \ cat ( -- )
  \
  \ Show a disk catalogue of the current drive.
  \
  \ See: `acat`, `?cat-fda`, `cat-fda`, `set-drive`.
  \
  \ }doc

( rename-file )

need file-dir# need get-filename need set-filename
need read-file-descriptor need write-file-descriptor

: rename-file ( ca1 len1 ca2 len2 -- ior )
  file-dir# nip 0= if 2drop #-1002 exit then
    \ If _ca2 len2_ already exists, exit with ior #-1002 (file exists).
  get-filename >stringer 2swap
    \ Get the complete version of _ca2 len2_ (with filetype),
    \ which was stored by `file-dir#` at `fda`, and preserve it.
  file-dir# ?dup if nip nip nip exit then
    \ If _ca1 len1_ does not exists, exit with _ior_.
  dup >r read-file-descriptor ?dup if rdrop 2drop exit then
    \ Read file descriptor of _ca1 len1_, to complete the data at `fda`.
    \ If read error, exit with whatever _ior_ is left.
  set-filename r> write-file-descriptor ;
    \ Patch the new filename and write `fda` to disk.

  \ doc{
  \
  \ rename-file ( ca1 len1 ca2 len2 -- ior )
  \
  \ Rename the file named by the character string _ca1 len1_ to
  \ the name in the character string _ca2 len2_, returning the
  \ I/O result code _ior_.
  \
  \ WARNING: TR-DOS uses the 9th character of filenames as the
  \ filetype identifier.  When the filetype is not specified in
  \ a filename, Solo Forth uses 'C' (code file) by default.
  \ ``rename-file`` does not check filetypes, so it can be used
  \ also to change the filetype. As usual in Forth, the
  \ programmer is supposed to know what he is doing. See the
  \ examples below.
  \
  \ Examples:
  \
  \ Given a BASIC program file saved as "old", the following
  \ instruction does not rename it to "new", because filetypes
  \ are not specified and filetype 'C' (code file) is used by
  \ default. Therefore a code file "old", if it exists, is
  \ renamed to "new":

  \ ----
  \ s" old" s" new" rename-file throw
  \ ----

  \ The following instruction renames a BASIC program "old" to
  \ "new", but since the filetype is not included in the new
  \ name, the default filetype 'C' (code file) is used. The
  \ "new" file will be a BASIC program marked as a code file:

  \ ----
  \ s" old     B" s" new" rename-file throw
  \ ----

  \ Including both filetypes is always safe:

  \ ----
  \ s" old     B" s" new     B" rename-file throw
  \ ----

  \ Origin: Forth-94 (FILE EXT), Forth-2012 (FILE EXT).
  \
  \ See: `file-status`, `delete-file`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

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
  \
  \ 2017-03-06: Add `>file`, which was removed from the kernel
  \ by mistake on 2017-02-12.
  \
  \ 2017-03-08: Rename `cat` to `acat`, after the names used in
  \ G+DOS. Add `(acat`. Improve calculation of dosior #1 ("no
  \ files"). Add `delete-file`.
  \
  \ 2017-03-10: Adapt the DOS calls to the changes in the
  \ kernel: The C register is not copied to the A register
  \ anymore after returning from the DOS call; instead, the A
  \ register is loaded from the TR-DOS latest error variable.
  \ Add draft of `undelete-file`.  Add `read-file-descriptor`,
  \ `write-file-descriptor`, `read-system-track`, `files/disk`.
  \ Improve documentation.
  \
  \ 2017-03-11: Finish `undelete-file`. Add `.filename`,
  \ `.fda-filename`, `fda-basic?`, `fda-deleted?`,
  \ `fda-empty?`, `cat`, `cat-fda`, `?cat-fda`. Improve
  \ documentation. Improve `undelete-file`: remove `throw` and
  \ factor with `undelete-fda`. Fix requirements of
  \ `undelete-file`. Rename `(file-dir#)` to `fda-filedir#` and
  \ fix it.  Rename `(file-status)` to `fda-filestatus`. Add
  \ `get-filename` and `rename-file`. Improve documentation.
  \
  \ 2017-03-12: Improve documentation.  Update the names of
  \ `stringer` words.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-21: Adapt to the new implementation of assembler
  \ labels.
  \
  \ 2017-12-09: Improve documentation.
  \
  \ 2018-01-11: Fix and document `read-system-track`.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.
  \
  \ 2018-03-02: Improve `cat`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-11: Add `2-block-drives`.
  \
  \ 2018-03-13: Fix typo.
  \
  \ 2018-04-05: Fix documentation of `2-block-drives`. Add
  \ `3-block-drives` and `4-block-drives`.
  \
  \ 2018-04-16: Improve description of _ior_ notation. Fix
  \ documentation.
  \
  \ 2018-06-04: Update: remove trailing closing paren from
  \ word names.

  \ vim: filetype=soloforth
