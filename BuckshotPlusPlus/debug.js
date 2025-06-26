#!/usr/bin/env node

const { execSync } = require('child_process');
const path = require('path');
const fs = require('fs');

// Get command line arguments
const args = process.argv.slice(2);

// Show help if no arguments or -h/--help is provided
if (args.length === 0 || args.includes('-h') || args.includes('--help')) {
    console.log(`
BuckshotPlusPlus Debug Runner

Usage: node debug.js <bpp-file> [options]

Options:
  -h, --help    Show this help message
  -v, --version Show version information
  --no-build    Skip the build step (use existing build)

Example:
  node debug.js examples/main.bpp
  `);
    process.exit(0);
}

// Show version if -v/--version is provided
if (args.includes('-v') || args.includes('--version')) {
    console.log('BuckshotPlusPlus Debug Runner v1.0.0');
    process.exit(0);
}

// Get the BPP file path (first non-option argument)
const bppFile = args.find(arg => !arg.startsWith('-'));
if (!bppFile) {
    console.error('Error: No BPP file specified');
    process.exit(1);
}

// Check if the file exists
const fullPath = path.resolve(bppFile);
if (!fs.existsSync(fullPath)) {
    console.error(`Error: File not found: ${fullPath}`);
    process.exit(1);
}

// Project root directory (where this script is located)
const projectRoot = __dirname;

// Build the project if --no-build is not specified
if (!args.includes('--no-build')) {
    console.log('🏗️  Building BuckshotPlusPlus...');
    try {
        execSync('dotnet build', { 
            cwd: projectRoot,
            stdio: 'inherit' 
        });
        console.log('✅ Build completed successfully!');
    } catch (error) {
        console.error('❌ Build failed');
        process.exit(1);
    }
}

// Run the compiled program with the BPP file
console.log(`🚀 Running ${bppFile}...\n`);
try {
    const dotnetRun = execSync(
        `dotnet run --project "${path.join(projectRoot)}" "${fullPath}"`,
        { stdio: 'inherit' }
    );
} catch (error) {
    console.error(`\n❌ Error running the program: ${error.message}`);
    process.exit(1);
}
