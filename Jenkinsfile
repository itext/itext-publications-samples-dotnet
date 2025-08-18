#!/usr/bin/env groovy
@Library('pipeline-library')_

def repoName = "samples"
def dependencyRegex = "(itextcore|html2pdf|typography|licensekey|cleanup|pdfxfa)"
def solutionFile = "itext.samples.sln"
def frameworksToTest = "net461"
def frameworksToTestForMainBranches = "net461;netcoreapp2.0"

automaticDotnetBuild(repoName, dependencyRegex, solutionFile, frameworksToTest, frameworksToTestForMainBranches)