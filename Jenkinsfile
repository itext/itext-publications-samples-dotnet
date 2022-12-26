#!/usr/bin/env groovy
@Library('pipeline-library')_

def repoName = "samples"
def dependencyRegex = "(itextcore|html2pdf|typography|licensekey|cleanup|pdfxfa)"
def solutionFile = "itext.samples.sln"
def csprojFramework = "netcoreapp2.0"

automaticDotnetBuild(repoName, dependencyRegex, solutionFile, csprojFramework)