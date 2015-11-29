require 'bundler/setup'

require 'albacore'
require 'albacore/tasks/versionizer'

Configuration = ENV['CONFIGURATION'] || 'Release'

Albacore::Tasks::Versionizer.new :versioning

desc 'create assembly infos'
asmver_files :assembly_info do |a|
  a.files = FileList['**/*.fsproj']
  a.attributes assembly_description: 'FSharp.TV web site',
               assembly_configuration: Configuration,
               assembly_company: 'Your Company',
               assembly_copyright: "(c) 2015 by Yourself!",
               assembly_version: ENV['LONG_VERSION'],
               assembly_file_version: ENV['LONG_VERSION'],
               assembly_informational_version: ENV['BUILD_VERSION']
end

task :paket_bootstrap do
system 'tools/paket.bootstrapper.exe', clr_command: true unless   File.exists? 'tools/paket.exe'
end

desc 'restore all nugets as per the packages.config files'
task :restore => :paket_bootstrap do
  system 'tools/paket.exe', 'restore', clr_command: true
end

desc 'Perform full build'
build :compile => [:versioning, :restore, :assembly_info] do |b|
  b.prop 'Configuration', Configuration
  b.sln = 'src/Web.sln'
end

directory 'build/pkg'

namespace :tests do
  task :unit do
    system "src/MyProj.Tests/bin/#{Configuration}/MyProj.Tests.exe", clr_command: true
  end
end

task :tests => :'tests:unit'

task :default => [:compile] #, :tests]
